using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Xmss : IXmss
{
    private readonly IShaFactory _shaFactory;
    private readonly IWots _wots;
    
    public Xmss(IShaFactory shaFactory, IWots wots)
    {
        _shaFactory = shaFactory;
        _wots = wots;
    }

    public byte[] XmssNode(byte[] skSeed, byte[] pkSeed, int i, int z, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: if z > h′ or i ≥ 2^(h′−z) then 
           2:   return NULL
           3: endif 
           4: if z = 0 then
           5:   ADRS.setTypeAndClear(WOTS_HASH) 
           6:   ADRS.setKeyPairAddress(i)
           7:   node ← wots_PKgen(SK.seed, PK.seed, ADRS)
           8: else
           9:   lnode ← xmss_node(SK.seed, 2i, z − 1, PK.seed, ADRS) 
           10:  rnode ← xmss_node(SK.seed, 2i + 1, z − 1, PK.seed, ADRS) 
           11:  ADRS.setTypeAndClear(TREE)
           12:  ADRS.setTreeHeight(z)
           13:  ADRS.setTreeIndex(i)
           14:  node ← H(PK.seed, ADRS, lnode ∥ rnode)
           15: endif 
           16: return node */
        byte[] node;
        
        // 1: if z > h′ or i ≥ 2^(h′−z) then 
        // 2:   return NULL
        if (z > slhdsaParameterSetAttributes.HPrime)
            throw new ArgumentException($"z > h′, i.e., {z} > {slhdsaParameterSetAttributes.HPrime}");
        if (i >= BigInteger.Pow(2, slhdsaParameterSetAttributes.HPrime - z))
            throw new ArgumentException($"i ≥ 2^(h′-z), i.e., {i} ≥ {BigInteger.Pow(2, slhdsaParameterSetAttributes.HPrime - z)}");        
        // 4: if z = 0 then
        if (z == 0)
        {
            // 5: ADRS.setTypeAndClear(WOTS_HASH) 
            // 6: ADRS.setKeyPairAddress(i)
            adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)i, adrs.KeyPairAddress.Length);
            // 7: node ← wots_PKgen(SK.seed, PK.seed, ADRS)
            node = _wots.WotsPKGen(skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);
        }
        else
        {
            // 9:  lnode ← xmss_node(SK.seed, 2i, z − 1, PK.seed, ADRS)
            var lNode = this.XmssNode(skSeed, pkSeed, 2 * i, z - 1, adrs, slhdsaParameterSetAttributes);
            //var lnode = XmssNode(skSeed, pkSeed, 2 * i, z - 1, adrs, slhdsaParameterSetAttributes);
            // 10: rnode ← xmss_node(SK.seed, 2i + 1, z − 1, PK.seed, ADRS) 
            var rNode = this.XmssNode(skSeed, pkSeed, (2 * i) + 1, z - 1, adrs, slhdsaParameterSetAttributes);
            // var rnode = XmssNode(skSeed, pkSeed, (2 * i) + 1, z - 1, adrs, slhdsaParameterSetAttributes);
            byte[] lAndRNodes = new byte[lNode.Length + rNode.Length];
            lNode.CopyTo(lAndRNodes, 0);
            rNode.CopyTo(lAndRNodes, lNode.Length);            
            // 11: ADRS.setTypeAndClear(TREE)
            var treeAdrs = new TreeAdrs(adrs.LayerAddress, adrs.TreeAddress);
            // 12: ADRS.setTreeHeight(z)
            treeAdrs.TreeHeight = SlhdsaHelpers.ToByte((ulong)z, treeAdrs.TreeHeight.Length);
            // 13: ADRS.setTreeIndex(i)
            treeAdrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)i, treeAdrs.TreeIndex.Length);
            // 14: node ← H(PK.seed, ADRS, lnode ∥ rnode)            
            var fHOrTFactory = new FHOrTFactory(_shaFactory);
            var h = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.H);
            node = h.Hash(pkSeed, treeAdrs, lAndRNodes);
        }
        // 16: return node 
        return node;
    }

    public byte[] XmssSign(byte[] M, byte[] skSeed, byte[] pkSeed, int idx, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: for j from 0 to h′ − 1 do  ▷ Build authentication path
           2:   k ← floor(idx / 2^j) ⊕ 1
           3:   AUTH[j] ← xmss_node(SK.seed, k, j, PK.seed, ADRS)
           4: end for
           5:
           6: ADRS.setTypeAndClear(WOTS_HASH)
           7: ADRS.setKeyPairAddress(idx)
           8: sig ← wots_sign(M, SK.seed, PK.seed, ADRS)
           9: SIGXMSS ← sig ∥ AUTH
           10: return SIGXMSS  */
        int k;
        byte[] auth = new byte[slhdsaParameterSetAttributes.HPrime * slhdsaParameterSetAttributes.N];

        // 1: for j from 0 to h′ − 1 do  ▷ Build authentication path
        for (int j = 0; j < slhdsaParameterSetAttributes.HPrime; j++)
        {
            // 2: k ← floor(idx / 2^j) ⊕ 1
            // Per Appendix B, floor(x/y) (<-- floating point arithmetic) = x/y (<-- integer division) when integer division is used
            k = (idx / (int) BigInteger.Pow(2, j)) ^ 1;
            // 3: AUTH[j] ← xmss_node(SK.seed, k, j, PK.seed, ADRS)
            XmssNode(skSeed, pkSeed, k, j, adrs, slhdsaParameterSetAttributes).CopyTo(auth, j * slhdsaParameterSetAttributes.N);
        }
        
        // 6: ADRS.setTypeAndClear(WOTS_HASH) <-- not needed as we're already working with an adrs of type WOTS_HASH
        // 7: ADRS.setKeyPairAddress(idx)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idx, adrs.KeyPairAddress.Length);
        // 8: sig ← wots_sign(M, SK.seed, PK.seed, ADRS)
        var sig = _wots.WotsSign(M, skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 9: SIGXMSS ← sig ∥ AUTH
        byte[] sigXmss = new byte[sig.Length + auth.Length];
        sig.CopyTo(sigXmss, 0);
        auth.CopyTo(sigXmss, sig.Length);
        // 10: return SIGXMSS
        return sigXmss;
    }

    public byte[] XmssPKFromSig(int idx, byte[] sigXmss, byte[] M, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: ADRS.setTypeAndClear(WOTS_HASH) ▷ Compute WOTS+ pk from WOTS+ sig
           2: ADRS.setKeyPairAddress(idx)
           3: sig ← SIGXMSS.getWOTSSig() ▷ SIGXMSS[0 : len · n]
           4: AUTH ← SIGXMSS.getXMSSAUTH() ▷ SIGXMSS[len · n : (len + h′) · n]
           5: node[0] ← wots_PKFromSig(sig, M, PK.seed, ADRS)
           6:
           7: ADRS.setTypeAndClear(TREE) ▷ Compute root from WOTS+ pk and AUTH
           8: ADRS.setTreeIndex(idx)
           9: for k from 0 to h′ − 1 do
           10:  ADRS.setTreeHeight(k + 1)
           11:  if floor( idx / 2^k) is even then
           12:      ADRS.setTreeIndex(ADRS.getTreeIndex()/2)
           13:      node[1] ← H(PK.seed, ADRS, node[0] ∥ AUTH[k])
           14:  else
           15:      ADRS.setTreeIndex((ADRS.getTreeIndex() − 1)/2)
           16:      node[1] ← H(PK.seed, ADRS, AUTH[k] ∥ node[0])
           17:  end if
           18:  node[0] ← node[1]
           19: end for
           20: return node[0]  */
        
        // 1: ADRS.setTypeAndClear(WOTS_HASH) ▷ Compute WOTS+ pk from WOTS+ sig
        // 2: ADRS.setKeyPairAddress(idx)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idx, adrs.KeyPairAddress.Length);
        // 3: sig ← SIGXMSS.getWOTSSig() ▷ SIGXMSS[0 : len · n]
        //var sig = new ArraySegment<byte>(sigXmss, 0, slhdsaParameterSetAttributes.Len * slhdsaParameterSetAttributes.N);
        var sig = sigXmss[..(slhdsaParameterSetAttributes.Len * slhdsaParameterSetAttributes.N)];
        // 4: AUTH ← SIGXMSS.getXMSSAUTH() ▷ SIGXMSS[len · n : (len + h′) · n]
        //var auth = new ArraySegment<byte>(sigXmss, slhdsaParameterSetAttributes.Len * slhdsaParameterSetAttributes.N,
        //    slhdsaParameterSetAttributes.HPrime * slhdsaParameterSetAttributes.N);
        var auth = sigXmss[(slhdsaParameterSetAttributes.Len * slhdsaParameterSetAttributes.N)..((slhdsaParameterSetAttributes.Len + slhdsaParameterSetAttributes.HPrime) * slhdsaParameterSetAttributes.N)]; 
        // 5: node[0] ← wots_PKFromSig(sig, M, PK.seed, ADRS)
        var node0 = _wots.WotsPKFromSig(sig, M, pkSeed, adrs, slhdsaParameterSetAttributes);
        byte[] node1, authK;
        
        // 7: ADRS.setTypeAndClear(TREE) ▷ Compute root from WOTS+ pk and AUTH
        var treeAdrs = new TreeAdrs(adrs.LayerAddress, adrs.TreeAddress);
        // 8: ADRS.setTreeIndex(idx)
        treeAdrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)idx, treeAdrs.TreeIndex.Length);
        
        byte[] node0AndAuthK = new byte[node0.Length + slhdsaParameterSetAttributes.N];
        var fHOrTFactory = new FHOrTFactory(_shaFactory);
        var h = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.H);
        // 9: for k from 0 to h′ − 1 do
        for (int k = 0; k < slhdsaParameterSetAttributes.HPrime; k++)
        {
            // 10: ADRS.setTreeHeight(k + 1)
            treeAdrs.TreeHeight = SlhdsaHelpers.ToByte((ulong)k + 1, treeAdrs.TreeHeight.Length);
            authK = auth[(k * slhdsaParameterSetAttributes.N)..((k + 1) * slhdsaParameterSetAttributes.N)];
            // 11: if floor( idx / 2^k) is even then
            // Per Appendix B, floor(x/y) (<-- floating point arithmetic) = x/y (<-- integer division) when integer division is used
            if ( (idx / (int) BigInteger.Pow(2, k)) % 2 == 0 )
            {
                // 12: ADRS.setTreeIndex(ADRS.getTreeIndex()/2)
                treeAdrs.TreeIndex = SlhdsaHelpers.ToByte(SlhdsaHelpers.ToInt(treeAdrs.TreeIndex, treeAdrs.TreeIndex.Length) / 2, treeAdrs.TreeIndex.Length);
                // 13: node[1] ← H(PK.seed, ADRS, node[0] ∥ AUTH[k]) 
                node0.CopyTo(node0AndAuthK, 0);
                authK.CopyTo(node0AndAuthK, node0.Length);
                node1 = h.Hash(pkSeed, treeAdrs, node0AndAuthK);
            }
            else
            {
               // 15: ADRS.setTreeIndex((ADRS.getTreeIndex() − 1)/2)
               treeAdrs.TreeIndex = SlhdsaHelpers.ToByte((SlhdsaHelpers.ToInt(treeAdrs.TreeIndex, treeAdrs.TreeIndex.Length) - 1) / 2, treeAdrs.TreeIndex.Length);
               // 16: node[1] ← H(PK.seed, ADRS, AUTH[k] ∥ node[0]) 
               authK.CopyTo(node0AndAuthK, 0);
               node0.CopyTo(node0AndAuthK, authK.Length);
               node1 = h.Hash(pkSeed, treeAdrs, node0AndAuthK);
            }
            // 18: node[0] ← node[1]
            node1.CopyTo(node0, 0);
        }
        // 20: return node[0]  
        return node0;
    }
}
