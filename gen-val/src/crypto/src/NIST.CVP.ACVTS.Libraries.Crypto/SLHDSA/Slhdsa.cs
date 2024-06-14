using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Slhdsa : ISlhdsa
{
    private readonly IShaFactory _shaFactory;
    private readonly IXmss _xmss;
    private readonly IHypertree _hypertree;
    private readonly IFors _fors;

    public Slhdsa(IShaFactory shaFactory, IXmss xmss, IHypertree hypertree, IFors fors)
    {
        _shaFactory = shaFactory;
        _xmss = xmss;
        _hypertree = hypertree;
        _fors = fors;
    }

    public KeyPair SlhKeyGen(byte[] nRandomBytesForSkSeed, byte[] nRandomBytesForSkPrf, byte[] nRandomBytesForPkSeed, 
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: SK.seed ←-$ B^n   ▷ Set SK.seed, SK.prf, and PK.seed to random n-byte
           2: SK.prf ←−$ B^n        ▷ strings using an approved random bit generator
           3: PK.seed ←−$ B^n
           4:
           5: ADRS ← toByte(0, 32) ▷ Generate the public key for the top-level XMSS tree
           6: ADRS.setLayerAddress(d − 1)
           7: PK.root ← xmss_node(SK.seed, 0, h′, PK.seed, ADRS)
           8:
           9: return ( (SK.seed, SK.prf, PK.seed, PK.root), (PK.seed, PK.root) ) */
        
        // nRandomBytesForSkSeed, nRandomBytesForSkPrf, and nRandomBytesForPkSeed should each be of length n
        string message = "";
        if (nRandomBytesForSkSeed.Length != slhdsaParameterSetAttributes.N)
            message += $"The length of {nameof(nRandomBytesForSkSeed)}, i.e., {nRandomBytesForSkSeed.Length} != n (n is {slhdsaParameterSetAttributes.N}). ";
        if (nRandomBytesForSkPrf.Length != slhdsaParameterSetAttributes.N)
            message += $"The length of {nameof(nRandomBytesForSkPrf)}, i.e., {nRandomBytesForSkPrf.Length} != n (n is {slhdsaParameterSetAttributes.N}). ";
        if (nRandomBytesForPkSeed.Length != slhdsaParameterSetAttributes.N)
            message += $"The length of {nameof(nRandomBytesForPkSeed)}, i.e., {nRandomBytesForPkSeed.Length} != n (n is {slhdsaParameterSetAttributes.N}). ";
        if (!string.IsNullOrEmpty(message))
            throw new ArgumentException(message);
     
        // 1: SK.seed ←-$ B^n   ▷ Set SK.seed, SK.prf, and PK.seed to random n-byte
        // 2: SK.prf ←−$ B^n        ▷ strings using an approved random bit generator
        // 3: PK.seed ←−$ B^n
        var skSeed = nRandomBytesForSkSeed;
        var skPrf = nRandomBytesForSkPrf;
        var pkSeed = nRandomBytesForPkSeed;
        
        // 5: ADRS ← toByte(0, 32) ▷ Generate the public key for the top-level XMSS tree
        var adrs = new WotsHashAdrs(); // WOTS_HASH is the address type with type = 0
        // 6: ADRS.setLayerAddress(d − 1)
        adrs.LayerAddress = SlhdsaHelpers.ToByte((ulong)slhdsaParameterSetAttributes.D - 1, adrs.LayerAddress.Length);
        // 7: PK.root ← xmss_node(SK.seed, 0, h′, PK.seed, ADRS)
        var pkRoot = _xmss.XmssNode(skSeed, pkSeed, 0, slhdsaParameterSetAttributes.HPrime, adrs,
            slhdsaParameterSetAttributes);
        // 9: return ( (SK.seed, SK.prf, PK.seed, PK.root), (PK.seed, PK.root) )
        PrivateKey privateKey = new PrivateKey(skSeed, skPrf, pkSeed, pkRoot);
        PublicKey publicKey = new PublicKey(pkSeed, pkRoot);
        return new KeyPair(privateKey, publicKey);
    }

    public byte[] SlhSignDeterministic(byte[] M, PrivateKey sk, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        return SlhSign(M, sk, true, Array.Empty<byte>(), slhdsaParameterSetAttributes);
    }

    public byte[] SlhSignNonDeterministic(byte[] M, PrivateKey sk, byte[] nRandomBytesForOptRand, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        // nRandomBytesForOptRand should be n bytes long 
        if (nRandomBytesForOptRand.Length != slhdsaParameterSetAttributes.N)
            throw new ArgumentException($"The length of {nameof(nRandomBytesForOptRand)}, i.e., {nRandomBytesForOptRand.Length} != n (n is {slhdsaParameterSetAttributes.N}). ");
        
        return SlhSign(M, sk, false, nRandomBytesForOptRand, slhdsaParameterSetAttributes);
    }

    /// <summary>
    ///  Generates an SLH-DSA signature
    /// </summary>
    /// <param name="M">The message for which to produce a signature</param>
    /// <param name="sk">SLH-DSA private key</param>
    /// <param name="deterministic">Whether randomization is used, i.e., whether opt_rand is set to a random value (vs to PK.seed)</param>
    /// <param name="nRandomBytesForOptRand">Random n byte value for opt_rand</param>
    /// <param name="slhdsaParameterSetAttributes">Information about/the values associated with the SLH-DSA parameter set in use</param>
    /// <returns>SLH-DSA signature</returns>
    private byte[] SlhSign(byte[] M, PrivateKey sk, bool deterministic, byte[] nRandomBytesForOptRand, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: ADRS ← toByte(0, 32)
           2:
           3: opt_rand ← PK.seed    ▷ Set opt_rand to either PK.seed
           4: if (RANDOMIZE) then   ▷ or to a random n-byte string
           5:   opt_rand ←$ B^n
           6: end if
           7: R ← PRFmsg(SK.prf, opt_rand, M)   ▷ Generate randomizer
           8: SIG ← R
           9:
           10: digest ← Hmsg(R, PK.seed, PK.root, M)    ▷ Compute message digest
           11: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
           12: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes 
           13: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes 
           14:
           15: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
           16: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
           17:
           18: ADRS.setTreeAddress(idxTree)
           19: ADRS.setTypeAndClear(FORS_TREE)
           20: ADRS.setKeyPairAddress(idxLeaf)
           21: SIGFORS ← fors_sign(md, SK.seed, PK.seed, ADRS)
           22: SIG ← SIG ∥ SIGFORS
           23:
           24: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS) ▷ Get FORS key
           25:
           26: SIGHT ← ht_sign(PKFORS, SK.seed, PK.seed, idxTree, idxLeaf)
           27: SIG ← SIG ∥ SIGHT
           28: return SIG */
        var prfMsgFactory = new PrfMsgFactory(_shaFactory);
        var prfMsg = prfMsgFactory.GetPrfMsg(slhdsaParameterSetAttributes);
        var hMsgFactory = new HMsgFactory(_shaFactory);
        var hMsg = hMsgFactory.GetHMsg(slhdsaParameterSetAttributes);
        
        // 1: ADRS ← toByte(0, 32) <-- this step isn't really needed for how we've implemented the ADRSes 
        
        // 3: opt_rand ← PK.seed    ▷ Set opt_rand to either PK.seed
        // 4: if (RANDOMIZE) then   ▷ or to a random n-byte string
        // 5:   opt_rand ←$ B^n
        var optRand = deterministic ? sk.PkSeed : nRandomBytesForOptRand;
        
        // 7: R ← PRFmsg(SK.prf, opt_rand, M)   ▷ Generate randomizer
        var r = prfMsg.GetPseudorandomByteString(sk.SkPrf, optRand, M);

        // 8: SIG ← R
        // according to FIPS 205 Section 9.2 Figure 16, an SLH-DSA signature is (k(1+a) + 1 + (h + d*len)) · n bytes in length
        var sigLen =
            (slhdsaParameterSetAttributes.K * (1 + slhdsaParameterSetAttributes.A) + 1 + (slhdsaParameterSetAttributes.H +
                slhdsaParameterSetAttributes.D * slhdsaParameterSetAttributes.Len)) * slhdsaParameterSetAttributes.N;
        byte[] sig = new byte[sigLen];
        r.CopyTo(sig, 0);
        
        // 10: digest ← Hmsg(R, PK.seed, PK.root, M)    ▷ Compute message digest
        var digest = hMsg.Hash(r, sk.PkSeed, sk.PkRoot, M);
        // 11: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        int start = 0;
        int end = (slhdsaParameterSetAttributes.K * slhdsaParameterSetAttributes.A + 8 - 1) / 8;
        var md = digest[start..end];
        // 12: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes
        start = end;
        end += ((slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H/slhdsaParameterSetAttributes.D) + 8 - 1)/8; 
        var tmpIdxTree = digest[start..end];
        // 13: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes
        start = end;
        end += (slhdsaParameterSetAttributes.H + 8*slhdsaParameterSetAttributes.D - 1)/(8*slhdsaParameterSetAttributes.D);
        var tmpIdxLeaf = digest[start..end];
        
        // 15: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
        BigInteger dividend = SlhdsaHelpers.ToInt(tmpIdxTree,((slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H/slhdsaParameterSetAttributes.D) + 8 - 1)/8);
        BigInteger divisor = BigInteger.Pow(2, slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H / slhdsaParameterSetAttributes.D);
        ulong idxTree = (ulong)(dividend % divisor);
        // 16: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
        dividend = SlhdsaHelpers.ToInt(tmpIdxLeaf, (slhdsaParameterSetAttributes.H + 8*slhdsaParameterSetAttributes.D - 1)/(8*slhdsaParameterSetAttributes.D));
        divisor = BigInteger.Pow(2, slhdsaParameterSetAttributes.H / slhdsaParameterSetAttributes.D);
        var idxLeaf = (int)(dividend % divisor); // it's safe to cast the result to an int b/c an XMSS tree will only ever have 2^hPrime WOTS+ keys. 
        // The largest hPrime is 9, so no more than 9 bits will ever be needed to represent an idxLeaf value.
        
        // 18: ADRS.setTreeAddress(idxTree)
        // 19: ADRS.setTypeAndClear(FORS_TREE)
        var adrs = new ForsTreeAdrs();
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        // 20: ADRS.setKeyPairAddress(idxLeaf)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idxLeaf, adrs.KeyPairAddress.Length);
        // 21: SIGFORS ← fors_sign(md, SK.seed, PK.seed, ADRS)
        var sigFors = _fors.ForsSign(md, sk.SkSeed, sk.PkSeed, adrs, slhdsaParameterSetAttributes);
        // 22: SIG ← SIG ∥ SIGFORS
        sigFors.CopyTo(sig, r.Length); // up until now, sig only contained r
        
        // 24: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS) ▷ Get FORS key
        var pkFors = _fors.ForsPKFromSig(sigFors, md, sk.PkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 26: SIGHT ← ht_sign(PKFORS, SK.seed, PK.seed, idxTree, idxLeaf)
        var sigHt = _hypertree.HypertreeSign(pkFors, sk.SkSeed, sk.PkSeed, idxTree, idxLeaf, slhdsaParameterSetAttributes);
        // 27: SIG ← SIG ∥ SIGHT
        sigHt.CopyTo(sig, r.Length + sigFors.Length); // up until now, sig has contained r and sigFors
        // 28: return SIG
        return sig;
    }

    public SlhdsaVerificationResult SlhVerify(byte[] M, byte[] sig, PublicKey pk, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: if |SIG| != (1 + k(1 + a) + h + d · len) · n then
           2:   return false
           3: end if
           4: ADRS ← toByte(0, 32)
           5: R ← SIG.getR()   ▷ SIG[0 : n]
           6: SIGFORS ← SIG.getSIG_FORS()  ▷ SIG[n : (1 + k(1 + a)) · n]
           7: SIGHT ← SIG.getSIG_HT()  ▷ SIG[(1 + k(1 + a)) · n : (1 + k(1 + a) + h + d · len) · n]
           8:
           9: digest ← Hmsg(R, PK.seed, PK.root, M)  ▷ Compute message digest
           10: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
           11: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes
           12: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes
           13:
           14: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
           15: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
           16:
           17: ADRS.setTreeAddress(idxTree)
           18: ADRS.setTypeAndClear(FORS_TREE)
           19: ADRS.setKeyPairAddress(idxLeaf)
           20:
           21: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS)
           22:
           23: return ht_verify(PKFORS, SIGHT, PK.seed, idxtree, idxleaf, PK.root) */
        var hMsgFactory = new HMsgFactory(_shaFactory);
        var hMsg = hMsgFactory.GetHMsg(slhdsaParameterSetAttributes);
        
        // 1: if |SIG| != (1 + k(1 + a) + h + d · len) · n then
        // 2:   return false
        var validSigLen =
            (1 + slhdsaParameterSetAttributes.K * (1 + slhdsaParameterSetAttributes.A) + slhdsaParameterSetAttributes.H +
                slhdsaParameterSetAttributes.D * slhdsaParameterSetAttributes.Len) * slhdsaParameterSetAttributes.N;
        if (sig.Length != validSigLen)
            return new SlhdsaVerificationResult(
                $"signature is not the expected length (expected: {validSigLen} bytes, provided: {sig.Length} bytes).");
        // 4: ADRS ← toByte(0, 32) <-- this step isn't really needed for how we've implemented the ADRSes
        
        // 5: R ← SIG.getR()   ▷ SIG[0 : n]
        int start = 0;
        int end = slhdsaParameterSetAttributes.N;
        var r = sig[start..end];
        // 6: SIGFORS ← SIG.getSIG_FORS()  ▷ SIG[n : (1 + k(1 + a)) · n]
        start = end;
        end = (1 + slhdsaParameterSetAttributes.K*(1 + slhdsaParameterSetAttributes.A))*slhdsaParameterSetAttributes.N;
        var sigFors = sig[start..end];
        // 7: SIGHT ← SIG.getSIG_HT()  ▷ SIG[(1 + k(1 + a)) · n : (1 + k(1 + a) + h + d · len) · n]
        start = end;
        var sigHt = sig[start..];
        
        // 9: digest ← Hmsg(R, PK.seed, PK.root, M)  ▷ Compute message digest
        var digest = hMsg.Hash(r, pk.PkSeed, pk.PkRoot, M);
        // 10: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        start = 0;
        end = (slhdsaParameterSetAttributes.K * slhdsaParameterSetAttributes.A + 8 - 1) / 8;
        var md = digest[start..end];
        // 11: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes
        start = end;
        end += ((slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H/slhdsaParameterSetAttributes.D) + 8 - 1)/8; 
        var tmpIdxTree = digest[start..end];
        // 12: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes
        start = end;
        end += (slhdsaParameterSetAttributes.H + 8*slhdsaParameterSetAttributes.D - 1)/(8*slhdsaParameterSetAttributes.D);
        var tmpIdxLeaf = digest[start..end];
        
        // 14: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
        BigInteger dividend = SlhdsaHelpers.ToInt(tmpIdxTree,((slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H/slhdsaParameterSetAttributes.D) + 8 - 1)/8);
        BigInteger divisor = BigInteger.Pow(2, slhdsaParameterSetAttributes.H - slhdsaParameterSetAttributes.H / slhdsaParameterSetAttributes.D);
        ulong idxTree = (ulong)(dividend % divisor);
        // 15: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
        dividend = SlhdsaHelpers.ToInt(tmpIdxLeaf, (slhdsaParameterSetAttributes.H + 8*slhdsaParameterSetAttributes.D - 1)/(8*slhdsaParameterSetAttributes.D));
        divisor = BigInteger.Pow(2, slhdsaParameterSetAttributes.H / slhdsaParameterSetAttributes.D);
        var idxLeaf = (int)(dividend % divisor); // it's safe to cast the result to an int b/c an XMSS tree will only ever have 2^hPrime WOTS+ keys. 
        // The largest hPrime is 9, so no more than 9 bits will ever be needed to represent an idxLeaf value.

        // 17: ADRS.setTreeAddress(idxTree)
        // 18: ADRS.setTypeAndClear(FORS_TREE)
        var adrs = new ForsTreeAdrs();
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        // 19: ADRS.setKeyPairAddress(idxLeaf)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idxLeaf, adrs.KeyPairAddress.Length);
        
        // 21: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS)
        var pkFors = _fors.ForsPKFromSig(sigFors, md, pk.PkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 23: return ht_verify(PKFORS, SIGHT, PK.seed, idxtree, idxleaf, PK.root)
        var success = _hypertree.HypertreeVerify(pkFors, sigHt, pk.PkSeed, idxTree, idxLeaf, pk.PkRoot, slhdsaParameterSetAttributes);
        return success
            ? new SlhdsaVerificationResult()
            : new SlhdsaVerificationResult("Hypertree signature verification failed, slhdsa signature is not valid"); 
    }
}
