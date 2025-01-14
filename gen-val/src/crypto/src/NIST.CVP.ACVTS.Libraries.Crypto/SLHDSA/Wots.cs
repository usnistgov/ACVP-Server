using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Wots : IWots
{
    private readonly IShaFactory _shaFactory;

    public Wots(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    public byte[] WotsPKGen(byte[] skSeed, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: skADRS ← ADRS ▷ Copy address to create key generation key address
           2: skADRS.setTypeAndClear(WOTS_PRF)
           3: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           4: for i from 0 to len − 1 do
           5: skADRS.setChainAddress(i)
           6: sk ← PRF(PK.seed, SK.seed, skADRS) ▷ Compute secret value for chain i
           7: ADRS.setChainAddress(i)
           8: tmp[i] ← chain(sk, 0, w − 1, PK.seed, ADRS) ▷ Compute public value for chain i
           9: end for
           10: wotspkADRS ← ADRS ▷ Copy address to create WOTS+public key address
           11: wotspkADRS.setTypeAndClear(WOTS_PK)
           12: wotspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           13: pk ← Tlen(PK.seed, wotspkADRS,tmp) ▷ Compress public key
           14: return pk  */

        var fHOrTFactory = new FHOrTFactory(_shaFactory); 
        // needed as for a parameter to our implementation of chain()
        var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
        var t = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.T);
        var prf = new PrfFactory(_shaFactory).GetPrf(slhdsaParameterSetAttributes);
        //byte[] sk, chainOut;
        byte[] tmp = new byte[slhdsaParameterSetAttributes.Len*slhdsaParameterSetAttributes.N];
        
        // 1: skADRS ← ADRS ▷ Copy address to create key generation key address
        // 2: skADRS.setTypeAndClear(WOTS_PRF)
        var skAdrs = new WotsPrfAdrs(adrs.LayerAddress, adrs.TreeAddress);
        // 3: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        adrs.KeyPairAddress.CopyTo(skAdrs.KeyPairAddress, 0);

        // 4: for i from 0 to len − 1 do
        for (int i = 0; i < slhdsaParameterSetAttributes.Len; i++)
        {
            // 5: skADRS.setChainAddress(i)
            skAdrs.ChainAddress = SlhdsaHelpers.ToByte((ulong)i, skAdrs.ChainAddress.Length);
            // 6: sk ← PRF(PK.seed, SK.seed, skADRS) ▷ Compute secret value for chain i
            var sk = prf.GetPseudorandomByteString(pkSeed, skSeed, skAdrs);
            // 7: ADRS.setChainAddress(i)
            adrs.ChainAddress = SlhdsaHelpers.ToByte((ulong)i, adrs.ChainAddress.Length);
            // 8: tmp[i] ← chain(sk, 0, w − 1, PK.seed, ADRS) ▷ Compute public value for chain i
            var chainOut = SlhdsaHelpers.Chain(sk, 0, slhdsaParameterSetAttributes.W - 1, pkSeed, adrs, slhdsaParameterSetAttributes, f);
            chainOut.CopyTo(tmp, i*slhdsaParameterSetAttributes.N);
        }

        // 10: wotspkADRS ← ADRS ▷ Copy address to create WOTS+public key address
        // 11: wotspkADRS.setTypeAndClear(WOTS_PK)
        var wotsPkAdrs = new WotsPkAdrs(adrs.LayerAddress, adrs.TreeAddress);
        // 12: wotspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        adrs.KeyPairAddress.CopyTo(wotsPkAdrs.KeyPairAddress, 0);
        // 13: pk ← Tlen(PK.seed, wotspkADRS,tmp) ▷ Compress public key
        // 14: return pk  
        return t.Hash(pkSeed, wotsPkAdrs, tmp);
    }

    public byte[] WotsSign(byte[] M, byte[] skSeed, byte[] pkSeed, WotsHashAdrs adrs,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1:  csum ← 0
           3:  msg ← base_2b(M, lgw, len1) ▷ Convert message to base w
           5:  for i from 0 to len1 − 1 do ▷ Compute checksum
           6:    csum ← csum + w − 1− msg[i]
           7:  end for
           9:  csum ← csum ≪ ((8 − ((len2·lgw) mod 8)) mod 8) ▷ For lgw = 4 left shift by 4
           10: msg ← msg ∥ base_2b(toByte(csum, ceilling(len2*lgw/8)),lgw, len2) ▷ Convert csum to base w 
           12: skADRS ← ADRS
           13: skADRS.setTypeAndClear(WOTS_PRF)
           14: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           15: for i from 0 to len − 1 do
           16:   skADRS.setChainAddress(i)
           17:   sk ← PRF(PK.seed, SK.seed, skADRS) ▷ Compute secret value for chain i
           18:   ADRS.setChainAddress(i)
           19:   sig[i] ← chain(sk, 0, msg[i], PK.seed, ADRS) ▷ Compute signature value for chain i
           20: end for
           21: return sig*/
        
        var fHOrTFactory = new FHOrTFactory(_shaFactory); 
        // needed as for a parameter to our implementation of chain()
        var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
        var prf = new PrfFactory(_shaFactory).GetPrf(slhdsaParameterSetAttributes);
        //byte[] sk, chainOut;
        byte[] sig = new byte[slhdsaParameterSetAttributes.Len*slhdsaParameterSetAttributes.N];
        
        // Lines 3 and 10 indicate that the size of msg must be len1 + len2 = len (see FIPS 205 Section 5)  
        int[] msg = new int[slhdsaParameterSetAttributes.Len];
        // 1:  csum ← 0
        int csum = 0;
        // 3:  msg ← base_2b(M, lgw, len1) ▷ Convert message to base w
        SlhdsaHelpers.Base2B(M, slhdsaParameterSetAttributes.LGw, slhdsaParameterSetAttributes.Len1).CopyTo(msg, 0);
        // 5:  for i from 0 to len1 − 1 do ▷ Compute checksum
        for (int i = 0; i < slhdsaParameterSetAttributes.Len1; i++)
            // 6:    csum ← csum + w − 1 − msg[i]
            csum = csum + slhdsaParameterSetAttributes.W - 1 - msg[i];
        // 9:  csum ← csum ≪ ((8 − ((len2·lgw) mod 8)) mod 8) ▷ For lgw = 4 left shift by 4
        // As of the 8/24/23 FIPS 205 draft, this should always evaluate to a left shift by 4. Make more efficient - just shift left by 4?  
        csum <<= ((8 - ((slhdsaParameterSetAttributes.Len2 * slhdsaParameterSetAttributes.LGw) % 8)) % 8);
        // 10: msg ← msg ∥ base_2b(toByte(csum, ceiling(len2*lgw/8)),lgw, len2) ▷ Convert csum to base w
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        SlhdsaHelpers.Base2B(
            SlhdsaHelpers.ToByte((ulong)csum,(slhdsaParameterSetAttributes.Len2 * slhdsaParameterSetAttributes.LGw + 8 - 1)/8),
            slhdsaParameterSetAttributes.LGw, 
            slhdsaParameterSetAttributes.Len2)
            .CopyTo(msg, slhdsaParameterSetAttributes.Len1);
        // 12: skADRS ← ADRS
        // 13: skADRS.setTypeAndClear(WOTS_PRF)
        // 14: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        var skAdrs = new WotsPrfAdrs(adrs.LayerAddress, adrs.TreeAddress);
        adrs.KeyPairAddress.CopyTo(skAdrs.KeyPairAddress, 0);
        // 15: for i from 0 to len − 1 do
        for (int i = 0; i < slhdsaParameterSetAttributes.Len; i++)
        {
            // 16: skADRS.setChainAddress(i)
            skAdrs.ChainAddress = SlhdsaHelpers.ToByte((ulong)i, skAdrs.ChainAddress.Length);
            // 17: sk ← PRF(PK.seed, SK.seed, skADRS) ▷ Compute secret value for chain i
            var sk = prf.GetPseudorandomByteString(pkSeed, skSeed, skAdrs);
            // 18: ADRS.setChainAddress(i)
            adrs.ChainAddress = SlhdsaHelpers.ToByte((ulong)i, adrs.ChainAddress.Length);
            // 19: sig[i] ← chain(sk, 0, msg[i], PK.seed, ADRS) ▷ Compute signature value for chain i
            var chainOut = SlhdsaHelpers.Chain(sk, 0, msg[i], pkSeed, adrs, slhdsaParameterSetAttributes, f);
            chainOut.CopyTo(sig, i*slhdsaParameterSetAttributes.N);
        }
        // 21: return sig
        return sig;
    }

    public byte[] WotsPKFromSig(byte[] sig, byte[] M, byte[] pkSeed, WotsHashAdrs adrs,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1:  csum ← 0
           3:  msg ← base_2b(M, lgw, len1) ▷ Convert message to base w
           5:  for i from 0 to len1 − 1 do ▷ Compute checksum
           6:    csum ← csum + w − 1− msg[i]
           7:  end for
           9:  csum ← csum ≪ ((8 − ((len2·lgw) mod 8)) mod 8) ▷ For lgw = 4 left shift by 4
           10: msg ← msg ∥ base_2b(toByte(csum, ceilling(len2*lgw/8)),lgw, len2) ▷ Convert csum to base w
           11: for i from 0 to len − 1 do
           12:   ADRS.setChainAddress(i)
           13:   tmp[i] ← chain(sig[i], msg[i], w − 1 − msg[i], PK.seed, ADRS)
           14: end for
           15: wotspkADRS ← ADRS
           16: wotspkADRS.setTypeAndClear(WOTS_PK)
           17: wotspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           18: pksig ← Tlen(PK.seed, wotspkADRS,tmp)
           19: return pksig*/
        
        var fHOrTFactory = new FHOrTFactory(_shaFactory); 
        // needed as for a parameter to our implementation of chain()
        var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
        var t = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.T);
        byte[] tmp = new byte[slhdsaParameterSetAttributes.Len*slhdsaParameterSetAttributes.N];
        byte[] sigI = new byte[slhdsaParameterSetAttributes.N];
        
        // Lines 3 and 10 indicate that the size of msg must be len1 + len2 = len (see FIPS 205 Section 5)  
        int[] msg = new int[slhdsaParameterSetAttributes.Len];
        // 1:  csum ← 0
        int csum = 0;
        // 3:  msg ← base_2b(M, lgw, len1) ▷ Convert message to base w
        SlhdsaHelpers.Base2B(M, slhdsaParameterSetAttributes.LGw, slhdsaParameterSetAttributes.Len1).CopyTo(msg, 0);
        // 5:  for i from 0 to len1 − 1 do ▷ Compute checksum
        for (int i = 0; i < slhdsaParameterSetAttributes.Len1; i++)
            // 6:    csum ← csum + w − 1 − msg[i]
            csum = csum + slhdsaParameterSetAttributes.W - 1 - msg[i];
        // 9:  csum ← csum ≪ ((8 − ((len2·lgw) mod 8)) mod 8) ▷ For lgw = 4 left shift by 4
        // As of the 8/24/23 FIPS 205 draft, this should always evaluate to a left shift by 4. Make more efficient - just shift left by 4?  
        csum <<= ((8 - ((slhdsaParameterSetAttributes.Len2 * slhdsaParameterSetAttributes.LGw) % 8)) % 8);
        // 10: msg ← msg ∥ base_2b(toByte(csum, ceiling(len2*lgw/8)),lgw, len2) ▷ Convert csum to base w
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        SlhdsaHelpers.Base2B(
                SlhdsaHelpers.ToByte((ulong)csum,(slhdsaParameterSetAttributes.Len2 * slhdsaParameterSetAttributes.LGw + 8 - 1)/8),
                slhdsaParameterSetAttributes.LGw, 
                slhdsaParameterSetAttributes.Len2)
            .CopyTo(msg, slhdsaParameterSetAttributes.Len1);
        // 11: for i from 0 to len − 1 do
        for (int i = 0; i < slhdsaParameterSetAttributes.Len; i++)
        {
            // 12: ADRS.setChainAddress(i)
            adrs.ChainAddress = SlhdsaHelpers.ToByte((ulong)i, adrs.ChainAddress.Length);
            // 13: tmp[i] ← chain(sig[i], msg[i], w − 1 − msg[i], PK.seed, ADRS)
            Array.Copy(sig, i*slhdsaParameterSetAttributes.N, sigI,
                0, slhdsaParameterSetAttributes.N);
            var chainOut = SlhdsaHelpers.Chain(sigI, msg[i], slhdsaParameterSetAttributes.W - 1 - msg[i], pkSeed, adrs, slhdsaParameterSetAttributes, f);
            chainOut.CopyTo(tmp, i*slhdsaParameterSetAttributes.N);
        }
        // 15: wotspkADRS ← ADRS
        // 16: wotspkADRS.setTypeAndClear(WOTS_PK)
        var wotsPkAdrs = new WotsPkAdrs(adrs.LayerAddress, adrs.TreeAddress);
        // 17: wotspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        adrs.KeyPairAddress.CopyTo(wotsPkAdrs.KeyPairAddress, 0);
        // 18: pksig ← Tlen(PK.seed, wotspkADRS,tmp)
        // 19: return pksig
        return t.Hash(pkSeed, wotsPkAdrs, tmp);
    }
    
}
