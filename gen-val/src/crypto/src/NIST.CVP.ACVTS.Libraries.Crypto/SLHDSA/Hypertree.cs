using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Hypertree : IHypertree
{
    private readonly IXmss _xmss;
    
    public Hypertree(IXmss xmss)
    {
        _xmss = xmss;
    }

    public byte[] HypertreeSign(byte[] M, byte[] skSeed, byte[] pkSeed, ulong idxTree, int idxLeaf,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: ADRS ← toByte(0, 32)
           2:
           3: ADRS.setTreeAddress(idxtree)
           4: SIGtmp ← xmss_sign(M, SK.seed, idxleaf, PK.seed, ADRS)
           5: SIGHT ← SIGtmp
           6: root ← xmss_PKFromSig(idxleaf, SIGtmp, M, PK.seed, ADRS)
           7: for j from 1 to d − 1 do
           8:   idxleaf ← idxtree mod 2^h′  ▷ h′ least significant bits of idxtree
           9:   idxtree ← idxtree ≫ h′      ▷ Remove least significant h′ bits from idxtree
           10:  ADRS.setLayerAddress( j)
           11:  ADRS.setTreeAddress(idxtree)
           12:  SIGtmp ← xmss_sign(root, SK.seed, idxleaf, PK.seed, ADRS)
           13:  SIGHT ← SIGHT ∥ SIGtmp
           14:  if j < d − 1 then
           15:      root ← xmss_PKFromSig(idxleaf, SIGtmp, root, PK.seed, ADRS)
           16:  end if
           17: end for
           18: return SIGHT */
        
        // 1: ADRS ← toByte(0, 32) <-- An address w/ type set to 0 is by definition of a WOTS_HASH address type
        var adrs = new WotsHashAdrs();
        // 3: ADRS.setTreeAddress(idxtree)
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        // 4: SIGtmp ← xmss_sign(M, SK.seed, idxleaf, PK.seed, ADRS)
        var sigTmp = _xmss.XmssSign(M, skSeed, pkSeed, idxLeaf, adrs, slhdsaParameterSetAttributes);
        // 5: SIGHT ← SIGtmp
        // according to FIPS 205 Section 7.1, a hypertree signature is (h + d · len) · n bytes in length
        byte[] sigHt = new byte[(slhdsaParameterSetAttributes.H + slhdsaParameterSetAttributes.D * slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N];
        sigTmp.CopyTo(sigHt, 0);
        // 6: root ← xmss_PKFromSig(idxleaf, SIGtmp, M, PK.seed, ADRS)
        var root = _xmss.XmssPKFromSig(idxLeaf, sigTmp, M, pkSeed, adrs, slhdsaParameterSetAttributes);
        // 7: for j from 1 to d − 1 do
        for (int j = 1; j < slhdsaParameterSetAttributes.D; j++)
        {
            // 8:  idxleaf ← idxtree mod 2^h′  ▷ h′ least significant bits of idxtree
            idxLeaf = (int)(idxTree % (ulong) BigInteger.Pow(2, slhdsaParameterSetAttributes.HPrime));
            // 9:  idxtree ← idxtree ≫ h′      ▷ Remove least significant h′ bits from idxtree
            idxTree >>= slhdsaParameterSetAttributes.HPrime;
            // 10: ADRS.setLayerAddress( j)
            adrs.LayerAddress = SlhdsaHelpers.ToByte((ulong)j, adrs.LayerAddress.Length);
            // 11: ADRS.setTreeAddress(idxtree)
            adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
            // 12: SIGtmp ← xmss_sign(root, SK.seed, idxleaf, PK.seed, ADRS)
            sigTmp = _xmss.XmssSign(root, skSeed, pkSeed, idxLeaf, adrs, slhdsaParameterSetAttributes);
            // 13: SIGHT ← SIGHT ∥ SIGtmp  
            // the length of one XMSS signature is (h′ + len) * n per FIPS 205, Section 7.1, Figure 12
            sigTmp.CopyTo(sigHt, j * (slhdsaParameterSetAttributes.HPrime + slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N);
            // 14: if j < d − 1 then
            if (j < slhdsaParameterSetAttributes.D - 1)
            {
                // 15: root ← xmss_PKFromSig(idxleaf, SIGtmp, root, PK.seed, ADRS)
                root = _xmss.XmssPKFromSig(idxLeaf, sigTmp, root, pkSeed, adrs, slhdsaParameterSetAttributes);
            }
        }
        // 18: return SIGHT
        return sigHt;
    }

    public bool HypertreeVerify(byte[] M, byte[] sigHt, byte[] pkSeed, ulong idxTree, int idxLeaf, byte[] pkRoot,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1:  ADRS ← toByte(0, 32)
           2:
           3:  ADRS.setTreeAddress(idxtree)
           4:  SIGtmp ← SIGHT.getXMSSSignature (0) ▷ SIGHT[0: (h′+ len) · n]
           5:  node ← xmss_PKFromSig(idxleaf, SIGtmp, M, PK.seed, ADRS)
           6:  for j from 1 to d − 1 do
           7:    idxleaf ← idxtree mod 2^h′  ▷ h′ least significant bits of idxtree
           8:    idxtree ← idxtree ≫ h′      ▷ Remove least significant h′ bits from idxtree
           9:    ADRS.setLayerAddress(j)
           10:   ADRS.setTreeAddress(idxtree)          
           11:   SIGtmp ← SIGHT.getXMSSSignature(j) ▷ SIGHT [ j · (h′+ len) · n : ( j + 1)(h′ + len) · n]
           12:   node ← xmss_PKFromSig(idxleaf, SIGtmp, node, PK.seed, ADRS)
           13: end for
           14: if node = PK.root then
           15:   return true
           16: else
           17:   return false
           18: end if */
        
        // 1: ADRS ← toByte(0, 32) <-- An address w/ type set to 0 is by definition of a WOTS_HASH address type
        var adrs = new WotsHashAdrs();
        // 3: ADRS.setTreeAddress(idxtree)
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        // 4: SIGtmp ← SIGHT.getXMSSSignature (0) ▷ SIGHT[0: (h′+ len) · n]
        var sigTmp = sigHt[..((slhdsaParameterSetAttributes.HPrime + slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N)];
        // 5: node ← xmss_PKFromSig(idxleaf, SIGtmp, M, PK.seed, ADRS)
        var node = _xmss.XmssPKFromSig(idxLeaf, sigTmp, M, pkSeed, adrs, slhdsaParameterSetAttributes);
        // 6: for j from 1 to d − 1 do
        for (int j = 1; j < slhdsaParameterSetAttributes.D; j++)
        {
            // 7:  idxleaf ← idxtree mod 2^h′  ▷ h′ least significant bits of idxtree
            idxLeaf = (int)(idxTree % (ulong) BigInteger.Pow(2, slhdsaParameterSetAttributes.HPrime));
            // 8:  idxtree ← idxtree ≫ h′      ▷ Remove least significant h′ bits from idxtree
            idxTree >>= slhdsaParameterSetAttributes.HPrime;
            // 9:  ADRS.setLayerAddress(j)
            adrs.LayerAddress = SlhdsaHelpers.ToByte((ulong)j, adrs.LayerAddress.Length);
            // 10: ADRS.setTreeAddress(idxtree)
            adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
            // 11: SIGtmp ← SIGHT.getXMSSSignature(j) ▷ SIGHT [ j · (h′+ len) · n : ( j + 1)(h′ + len) · n]
            sigTmp = sigHt[(j * (slhdsaParameterSetAttributes.HPrime + slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N)..((j + 1) * (slhdsaParameterSetAttributes.HPrime + slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N)];
            // 12: node ← xmss_PKFromSig(idxleaf, SIGtmp, node, PK.seed, ADRS)
            node = _xmss.XmssPKFromSig(idxLeaf, sigTmp, node, pkSeed, adrs, slhdsaParameterSetAttributes);
        }
        
        // 14: if node = PK.root then
        if (BitConverter.ToString(node).Equals(BitConverter.ToString(pkRoot)))
            // 15:   return true
            return true;
        // 16: else
        // 17:   return false
        return false;
    }
}
