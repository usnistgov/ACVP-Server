using System;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.ExternalInterfaces;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Slhdsa : ExternalSignatureBase, ISlhdsa
{
    private readonly IShaFactory _shaFactory;
    private readonly IXmss _xmss;
    private readonly IHypertree _hypertree;
    private readonly IFors _fors;
    private readonly SlhdsaParameterSetAttributes _params;
    private readonly IEntropyProvider _entropyProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slhdsaParameterSetAttributes"></param>
    /// <param name="shaFactory"></param>
    /// <param name="entropyProvider">Only used for non-deterministic signing</param>
    public Slhdsa(SlhdsaParameterSetAttributes slhdsaParameterSetAttributes, IShaFactory shaFactory, IEntropyProvider entropyProvider = null) : base(shaFactory)
    {
        _shaFactory = shaFactory;
        _xmss = new Xmss(_shaFactory, new Wots(_shaFactory));
        _hypertree = new Hypertree(_xmss);
        _fors = new Fors(_shaFactory);
        _params = slhdsaParameterSetAttributes;
        _entropyProvider = entropyProvider;
    }

    protected override byte[] GetRnd(bool deterministic)
    {
        return deterministic ? Array.Empty<byte>() : _entropyProvider.GetEntropy(_params.N).ToBytes();      // Empty byte[] sucks, but there isn't another way because the sk contains the actual value needed
    }

    public KeyPair SlhKeyGen(byte[] nRandomBytesForSkSeed, byte[] nRandomBytesForSkPrf, byte[] nRandomBytesForPkSeed)
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
        if (nRandomBytesForSkSeed.Length != _params.N)
            message += $"The length of {nameof(nRandomBytesForSkSeed)}, i.e., {nRandomBytesForSkSeed.Length} != n (n is {_params.N}). ";
        if (nRandomBytesForSkPrf.Length != _params.N)
            message += $"The length of {nameof(nRandomBytesForSkPrf)}, i.e., {nRandomBytesForSkPrf.Length} != n (n is {_params.N}). ";
        if (nRandomBytesForPkSeed.Length != _params.N)
            message += $"The length of {nameof(nRandomBytesForPkSeed)}, i.e., {nRandomBytesForPkSeed.Length} != n (n is {_params.N}). ";
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
        adrs.LayerAddress = SlhdsaHelpers.ToByte((ulong)_params.D - 1, adrs.LayerAddress.Length);
        
        // 7: PK.root ← xmss_node(SK.seed, 0, h′, PK.seed, ADRS)
        var pkRoot = _xmss.XmssNode(skSeed, pkSeed, 0, _params.HPrime, adrs, _params);
        
        // 9: return ( (SK.seed, SK.prf, PK.seed, PK.root), (PK.seed, PK.root) )
        PrivateKey privateKey = new PrivateKey(skSeed, skPrf, pkSeed, pkRoot);
        PublicKey publicKey = new PublicKey(pkSeed, pkRoot);
        return new KeyPair(privateKey, publicKey);
    }

    /// <summary>
    ///  Generates an SLH-DSA signature
    /// </summary>
    /// <param name="message">The message for which to produce a signature</param>
    /// <param name="sk">SLH-DSA private key</param>
    /// <param name="rnd">Whether randomization is used, i.e., whether opt_rand is set to a random value (vs to PK.seed)</param>
    /// <returns>SLH-DSA signature</returns>
    public override byte[] Sign(byte[] sk, byte[] message, byte[] rnd)
    {
        var privateKey = new PrivateKey(sk);
        var deterministic = rnd.Length == 0;   // Check if deterministic by seeing if rnd is empty, this is how it would come from the GetRnd()
        
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
        var prfMsg = prfMsgFactory.GetPrfMsg(_params);
        var hMsgFactory = new HMsgFactory(_shaFactory);
        var hMsg = hMsgFactory.GetHMsg(_params);
        
        // 1: ADRS ← toByte(0, 32) <-- this step isn't really needed for how we've implemented the ADRSes 
        
        // 3: opt_rand ← PK.seed    ▷ Set opt_rand to either PK.seed
        // 4: if (RANDOMIZE) then   ▷ or to a random n-byte string
        // 5:   opt_rand ←$ B^n
        var optRand = deterministic ? privateKey.PkSeed : rnd;
        // Console.WriteLine("optRand = " + IntermediateValueHelper.Print(optRand));
        
        // 7: R ← PRFmsg(SK.prf, opt_rand, M)   ▷ Generate randomizer
        var r = prfMsg.GetPseudorandomByteString(privateKey.SkPrf, optRand, message);
        // Console.WriteLine("r = " + IntermediateValueHelper.Print(r));
        
        // 8: SIG ← R
        // according to FIPS 205 Section 9.2 Figure 16, an SLH-DSA signature is (k(1+a) + 1 + (h + d*len)) · n bytes in length
        var sigLen = (_params.K * (1 + _params.A) + 1 + (_params.H + _params.D * _params.Len)) * _params.N;
        byte[] sig = new byte[sigLen];
        r.CopyTo(sig, 0);
        // Console.WriteLine("sig = " + IntermediateValueHelper.Print(sig));
        
        // 10: digest ← Hmsg(R, PK.seed, PK.root, M)    ▷ Compute message digest
        var digest = hMsg.Hash(r, privateKey.PkSeed, privateKey.PkRoot, message);
        // Console.WriteLine("digest = " + IntermediateValueHelper.Print(digest));
        
        // 11: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        int start = 0;
        int end = (_params.K * _params.A + 8 - 1) / 8;
        var md = digest[start..end];
        // Console.WriteLine("md = " + IntermediateValueHelper.Print(md));
        
        // 12: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes
        start = end;
        end += ((_params.H - _params.H/_params.D) + 8 - 1)/8; 
        var tmpIdxTree = digest[start..end];
        // Console.WriteLine("tmpIdfTree = " + IntermediateValueHelper.Print(tmpIdxTree));
        
        // 13: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes
        start = end;
        end += (_params.H + 8*_params.D - 1)/(8*_params.D);
        var tmpIdxLeaf = digest[start..end];
        // Console.WriteLine("tmpIdxLeaf = " + IntermediateValueHelper.Print(tmpIdxLeaf));
        
        // 15: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
        BigInteger dividend = SlhdsaHelpers.ToInt(tmpIdxTree,((_params.H - _params.H/_params.D) + 8 - 1)/8);
        BigInteger divisor = BigInteger.Pow(2, _params.H - _params.H / _params.D);
        ulong idxTree = (ulong)(dividend % divisor);
        // Console.WriteLine("idxTree = " + idxTree);
        
        // 16: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
        dividend = SlhdsaHelpers.ToInt(tmpIdxLeaf, (_params.H + 8*_params.D - 1)/(8*_params.D));
        divisor = BigInteger.Pow(2, _params.H / _params.D);
        var idxLeaf = (int)(dividend % divisor); // it's safe to cast the result to an int b/c an XMSS tree will only ever have 2^hPrime WOTS+ keys. 
        // The largest hPrime is 9, so no more than 9 bits will ever be needed to represent an idxLeaf value.
        // Console.WriteLine("idxLeaf = " + idxLeaf);
        
        // 18: ADRS.setTreeAddress(idxTree)
        // 19: ADRS.setTypeAndClear(FORS_TREE)
        var adrs = new ForsTreeAdrs();
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        // Console.WriteLine("adrs.TreeAddress = " + IntermediateValueHelper.Print(adrs.TreeAddress));
        
        // 20: ADRS.setKeyPairAddress(idxLeaf)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idxLeaf, adrs.KeyPairAddress.Length);
        // Console.WriteLine("adrs.KeyPairAddress = " + IntermediateValueHelper.Print(adrs.KeyPairAddress));
        
        // 21: SIGFORS ← fors_sign(md, SK.seed, PK.seed, ADRS)
        var sigFors = _fors.ForsSign(md, privateKey.SkSeed, privateKey.PkSeed, adrs, _params);
        // Console.WriteLine("sigFors = " + IntermediateValueHelper.Print(sigFors));
        
        // 22: SIG ← SIG ∥ SIGFORS
        sigFors.CopyTo(sig, r.Length); // up until now, sig only contained r
        // Console.WriteLine("sig = " + IntermediateValueHelper.Print(sig));
        
        // 24: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS) ▷ Get FORS key
        var pkFors = _fors.ForsPKFromSig(sigFors, md, privateKey.PkSeed, adrs, _params);
        // Console.WriteLine("pkFors = " + IntermediateValueHelper.Print(pkFors));

        // 26: SIGHT ← ht_sign(PKFORS, SK.seed, PK.seed, idxTree, idxLeaf)
        var sigHt = _hypertree.HypertreeSign(pkFors, privateKey.SkSeed, privateKey.PkSeed, idxTree, idxLeaf, _params);
        // Console.WriteLine("sigHt = " + IntermediateValueHelper.Print(sigHt));

        // 27: SIG ← SIG ∥ SIGHT
        sigHt.CopyTo(sig, r.Length + sigFors.Length); // up until now, sig has contained r and sigFors
        // Console.WriteLine("sig = " + IntermediateValueHelper.Print(sig));

        // 28: return SIG
        return sig;
    }

    public override bool Verify(byte[] pk, byte[] M, byte[] sig)
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
        var pubKey = new PublicKey(pk);
        var hMsgFactory = new HMsgFactory(_shaFactory);
        var hMsg = hMsgFactory.GetHMsg(_params);
        
        // 1: if |SIG| != (1 + k(1 + a) + h + d · len) · n then
        // 2:   return false
        var validSigLen = (1 + _params.K * (1 + _params.A) + _params.H + _params.D * _params.Len) * _params.N;
        if (sig.Length != validSigLen)
        {
            return false;
        }
        
        // 4: ADRS ← toByte(0, 32) <-- this step isn't really needed for how we've implemented the ADRSes
        
        // 5: R ← SIG.getR()   ▷ SIG[0 : n]
        int start = 0;
        int end = _params.N;
        var r = sig[start..end];
        
        // 6: SIGFORS ← SIG.getSIG_FORS()  ▷ SIG[n : (1 + k(1 + a)) · n]
        start = end;
        end = (1 + _params.K*(1 + _params.A))*_params.N;
        var sigFors = sig[start..end];
        
        // 7: SIGHT ← SIG.getSIG_HT()  ▷ SIG[(1 + k(1 + a)) · n : (1 + k(1 + a) + h + d · len) · n]
        start = end;
        var sigHt = sig[start..];
        
        // 9: digest ← Hmsg(R, PK.seed, PK.root, M)  ▷ Compute message digest
        var digest = hMsg.Hash(r, pubKey.PkSeed, pubKey.PkRoot, M);
        
        // 10: md ← digest[0 : ceiling(k·a/8)]  ▷ first ceiling(k·a/8) bytes
        // Note: from Appendix B, we have that ceiling(x/y)<-- using normal/floating point arithmetic = (x+y-1)/y <-- using integer arithmetic/division
        start = 0;
        end = (_params.K * _params.A + 8 - 1) / 8;
        var md = digest[start..end];
        
        // 11: tmp_idxTree ← digest[ceiling(k·a/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8)]    ▷ next ceiling( (h - h/d)/8) bytes
        start = end;
        end += ((_params.H - _params.H/_params.D) + 8 - 1)/8; 
        var tmpIdxTree = digest[start..end];
        
        // 12: tmp_idxLeaf ← digest[ceiling(k·a/8) + ceiling( (h - h/d)/8) : ceiling(k·a/8) + ceiling( (h - h/d)/8) + ceiling(h/8d)] ▷ next ceiling(h/8d) bytes
        start = end;
        end += (_params.H + 8*_params.D - 1)/(8*_params.D);
        var tmpIdxLeaf = digest[start..end];
        
        // 14: idxTree ← toInt(tmp_idxTree, ceiling( (h - h/d)/8)) mod 2^(h-h/d)
        BigInteger dividend = SlhdsaHelpers.ToInt(tmpIdxTree,((_params.H - _params.H/_params.D) + 8 - 1)/8);
        BigInteger divisor = BigInteger.Pow(2, _params.H - _params.H / _params.D);
        ulong idxTree = (ulong)(dividend % divisor);
        
        // 15: idxLeaf ← toInt(tmp_idxLeaf, ceiling(h/8d)) mod 2^(h/d)
        dividend = SlhdsaHelpers.ToInt(tmpIdxLeaf, (_params.H + 8*_params.D - 1)/(8*_params.D));
        divisor = BigInteger.Pow(2, _params.H / _params.D);
        var idxLeaf = (int)(dividend % divisor); // it's safe to cast the result to an int b/c an XMSS tree will only ever have 2^hPrime WOTS+ keys. 
        // The largest hPrime is 9, so no more than 9 bits will ever be needed to represent an idxLeaf value.

        // 17: ADRS.setTreeAddress(idxTree)
        // 18: ADRS.setTypeAndClear(FORS_TREE)
        var adrs = new ForsTreeAdrs();
        adrs.TreeAddress = SlhdsaHelpers.ToByte(idxTree, adrs.TreeAddress.Length);
        
        // 19: ADRS.setKeyPairAddress(idxLeaf)
        adrs.KeyPairAddress = SlhdsaHelpers.ToByte((ulong)idxLeaf, adrs.KeyPairAddress.Length);
        
        // 21: PKFORS ← fors_pkFromSig(SIGFORS, md, PK.seed, ADRS)
        var pkFors = _fors.ForsPKFromSig(sigFors, md, pubKey.PkSeed, adrs, _params);
        
        // 23: return ht_verify(PKFORS, SIGHT, PK.seed, idxtree, idxleaf, PK.root)
        return _hypertree.HypertreeVerify(pkFors, sigHt, pubKey.PkSeed, idxTree, idxLeaf, pubKey.PkRoot, _params);
    }
}
