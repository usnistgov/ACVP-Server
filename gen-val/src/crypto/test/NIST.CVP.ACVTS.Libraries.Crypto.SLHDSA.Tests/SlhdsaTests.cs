using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class SlhdsaTests
{
    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    private ISha _sha256;
    private IWots _wots;
    private IXmss _xmss;
    private IHypertree _hypertree;
    private IFors _fors;
    private Slhdsa _subject;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _sha256 = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
        _wots = new Wots(_shaFactory);
        _xmss = new Xmss(_shaFactory, _wots);
        _hypertree = new Hypertree(_xmss);
        _fors = new Fors(_shaFactory);
        _subject = new Slhdsa(_shaFactory, _xmss, _hypertree, _fors);
    }

    [Test]
    [TestCase("nRandomBytesForSkSeedLen, nRandomBytesForSkPrfLen and nRandomBytesForPkSeedLen != n ", SlhdsaParameterSet.SLH_DSA_SHA2_128s, 10, 20, 30)]
    [TestCase("nRandomBytesForSkPrfLen != n ", SlhdsaParameterSet.SLH_DSA_SHA2_128s, 16, 20, 16)]
    [TestCase("nRandomBytesForPkSeedLen != n ", SlhdsaParameterSet.SLH_DSA_SHA2_128s, 16, 16, 30)]
    public void ShouldFailSlhdsaKeyGenWithInValidParameters(string description, SlhdsaParameterSet slhdsaParameterSet, int nRandomBytesForSkSeedLen, int nRandomBytesForSkPrfLen, int nRandomBytesForPkSeedLen)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);

        byte[] nRandomBytesForSkSeed = new byte[nRandomBytesForSkSeedLen];
        byte[] nRandomBytesForSkPrf = new byte[nRandomBytesForSkPrfLen];
        byte[] nRandomBytesForPkSeed = new byte[nRandomBytesForPkSeedLen];
        
        // build SkSeed
        for (int i = 0; i < nRandomBytesForSkSeedLen; i++)
            nRandomBytesForSkSeed[i] = 0x1f;
        
        // build SkPrf
        for (int i = 0; i < nRandomBytesForSkPrfLen; i++)
            nRandomBytesForSkPrf[i] = 0x2e;
        
        // build PkSeed
        for (int i = 0; i < nRandomBytesForPkSeedLen; i++)
            nRandomBytesForPkSeed[i] = 0x3d;
        
        Assert.Throws<ArgumentException>(() => _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed, 
         slhdsaParameterSetAttributes));
        
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, "1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3DAF06062CA627D3E4C6E247B1906B18093D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3DAF06062CA627D3E4C6E247B1906B1809")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, "1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D23FCDD873E390872FA2A3ACAEA5905511648F4EFB0C276583D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D23FCDD873E390872FA2A3ACAEA5905511648F4EFB0C27658")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, "1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E2E3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3DAEA9EBA7772403525C2DA10917D72948BF7257CC25D6AA5467CE2E469CCB72CF3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3DAEA9EBA7772403525C2DA10917D72948BF7257CC25D6AA5467CE2E469CCB72CF")]
    public void ShouldSlhdsaKeyGenWithValidParameters(SlhdsaParameterSet slhdsaParameterSet, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        // build the inputs to SlhdsaKeyGen
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
        }
        
        // do the keyGen
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);

        // create BitStrings that are representative of the components of the private and public keys
        var privateKeyBitString = new BitString(keyPair.PrivateKey.SkSeed).ConcatenateBits(new BitString(keyPair.PrivateKey.SkPrf)).ConcatenateBits(new BitString(keyPair.PrivateKey.PkSeed)).ConcatenateBits(new BitString(keyPair.PrivateKey.PkRoot));
        var publicKeyBitString = new BitString(keyPair.PublicKey.PkSeed).ConcatenateBits(new BitString(keyPair.PublicKey.PkRoot));
        var keyPairBitString = BitString.ConcatenateBits(privateKeyBitString, publicKeyBitString);

        // Was the expected key pair generated?
        Assert.That(keyPairBitString.ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 10,"5693755D0A4E0DD83D260FE37183B765DA99FAA0AC2C167D61FB9C3341974977")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 60, "53286CD2F4A894AFC66CE2D69C31488B84FBB6B175AD9825B9FB96C16D0EFCDD")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 64, "84C7CE883A575198ECBBCF66851CD94F63D27B6A54BDEF1764FFD07409825566")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 512,"8135615F6566E19451AB8E87CB9ADD571B29B5865663E35AA984160F68C72756")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 512, "511082892341702DD3796D2405A6A581FBBD9FDD370789E8CFB64BE1FEC195AA")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 4096,"A2D234D6CB431E6682FE7231FE9E10A5FF351DB7F8FE30CB565A3F1A5041597B")]
    public void ShouldSlhdsaSignDeterministicKat(SlhdsaParameterSet slhdsaParameterSet, int messageLength, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        
        // create a message to sign
        byte[] message = new byte[messageLength];
        // build the seeds and message
        for (int i = 0; i < message.Length; i++)
            message[i] = 0xF0;
        
        // build the inputs to SlhdsaKeyGen
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
        }
        
        // create the key pair
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);
        
        // calculate the slhdsa signature of message
        var sig = _subject.SlhSignDeterministic(message, keyPair.PrivateKey, slhdsaParameterSetAttributes);
        // SLH-DSA signatures are large; hash the signature to get a smaller representative of the signature
        var hashResult = _sha256.HashMessage(new BitString(sig));
        var resultDigest = hashResult.Digest.ToHex(); 
        Console.WriteLine($"sig: {resultDigest}");

        // was the correct signature generated?
        Assert.That(resultDigest, Is.EqualTo(new BitString(expected).ToHex()));
    }
    
    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 20,"9BDD76776A7291F345EB55A01D13C99C09BCC64725C8B0BA366C151FAD55ECA5")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 30,"00A3F8A354AC015064A5F3E4384CF6AAD91299347DE0BA2DE53D375114979D54")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 128, "85728E6ED638BC8A72D1A4A94BADA2A665172FEB2B31EDCA1FF9CA352403CD6B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 256, "961511FC705D2A6FAEB8D945F5589DA73DA4FC8BB57584AD0EB09632F73BF573")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 1024, "EB7F66B5C41C559A2F24DEC4FA9C6045492A18EFF2208B988BF509AEDB2FE23E")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 2048,"4250CDD663FB2FD50B29C48CF4F412514CBEE839176595F0C33E9134302C1729")]
    public void ShouldSlhdsaSignNonDeterministicKat(SlhdsaParameterSet slhdsaParameterSet, int messageLength, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        
        // create a message to sign
        byte[] message = new byte[messageLength];
        for (int i = 0; i < message.Length; i++)
            message[i] = 0xF0;
        
        // build the inputs to SlhdsaKeyGen and nRandomBytesForOptRand
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForOptRand = new byte[slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
            nRandomBytesForOptRand[i] = 0x4c;
        }
        
        // create the key pair
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);
        
        // calculate the slhdsa signature of message
        var sig = _subject.SlhSignNonDeterministic(message, keyPair.PrivateKey, nRandomBytesForOptRand, slhdsaParameterSetAttributes);
        // SLH-DSA signatures are large; hash the signature to get a smaller representative of the signature
        var hashResult = _sha256.HashMessage(new BitString(sig));
        var resultDigest = hashResult.Digest.ToHex(); 
        Console.WriteLine($"sig: {resultDigest}");

        // was the correct signature generated?
        Assert.That(resultDigest, Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s,false)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, false)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, false)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, false)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, false)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, false)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, true)]
    public void ShouldSlhdsaSignAndVerify(SlhdsaParameterSet slhdsaParameterSet, bool deterministic)
    {
        byte[] sig;
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        
        // create a message to sign
        byte[] message = new byte[2048];
        for (int i = 0; i < message.Length; i++)
            message[i] = 0xF0;
        
        // build the inputs to SlhdsaKeyGen and nRandomBytesForOptRand
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForOptRand = new byte[slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
            nRandomBytesForOptRand[i] = 0x4c;
        }
        
        // create the key pair
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);
        
        // calculate the slhdsa signature of message
        if (deterministic)
        {
            sig = _subject.SlhSignNonDeterministic(message, keyPair.PrivateKey, nRandomBytesForOptRand, slhdsaParameterSetAttributes);    
        }
        else
        {
            sig = _subject.SlhSignDeterministic(message, keyPair.PrivateKey, slhdsaParameterSetAttributes);
        }

        var result = _subject.SlhVerify(message, sig, keyPair.PublicKey, slhdsaParameterSetAttributes);
        Assert.That(result.Success, Is.EqualTo(true));
        
        
    }

    [Test]
    [TestCase("altered message")]
    public void ShouldFailSlhVerifyForAlteredMessage(string scenario)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(SlhdsaParameterSet.SLH_DSA_SHA2_128f);
        
        // create a message to sign
        byte[] message = new byte[2048];
        for (int i = 0; i < message.Length; i++)
            message[i] = 0xF0;
        
        // build the inputs to SlhdsaKeyGen and nRandomBytesForOptRand
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForOptRand = new byte[slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
            nRandomBytesForOptRand[i] = 0x4c;
        }
        
        // create the key pair
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);
        
        // create the slhdsa signature
        var sig = _subject.SlhSignNonDeterministic(message, keyPair.PrivateKey, nRandomBytesForOptRand, slhdsaParameterSetAttributes);
        
        // alter the message
        message[0] = 0x00;
        
        var result = _subject.SlhVerify(message, sig, keyPair.PublicKey, slhdsaParameterSetAttributes);
        
        Assert.That(result.Success, Is.EqualTo(false));
    }
   
    [Test]
    [TestCase("altered signature")]
    public void ShouldFailSlhVerifyForAlteredSignature(string scenario)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(SlhdsaParameterSet.SLH_DSA_SHA2_128f);
        
        // create a message to sign
        byte[] message = new byte[2048];
        for (int i = 0; i < message.Length; i++)
            message[i] = 0xF0;
        
        // build the inputs to SlhdsaKeyGen and nRandomBytesForOptRand
        byte[] nRandomBytesForSkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForSkPrf = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForPkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] nRandomBytesForOptRand = new byte[slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            nRandomBytesForSkSeed[i] = 0x1f;
            nRandomBytesForSkPrf[i] = 0x2e;
            nRandomBytesForPkSeed[i] = 0x3d;
            nRandomBytesForOptRand[i] = 0x4c;
        }
        
        // create the key pair
        var keyPair = _subject.SlhKeyGen(nRandomBytesForSkSeed, nRandomBytesForSkPrf, nRandomBytesForPkSeed,
            slhdsaParameterSetAttributes);
        
        // create the slhdsa signature
        var sig = _subject.SlhSignNonDeterministic(message, keyPair.PrivateKey, nRandomBytesForOptRand, slhdsaParameterSetAttributes);
        
        // alter the signature
        if (sig[0] != 0x00)
            sig[0] = 0x00;
        else
            sig[0] = 0xFF;
        
        var result = _subject.SlhVerify(message, sig, keyPair.PublicKey, slhdsaParameterSetAttributes);
        
        Assert.That(result.Success, Is.EqualTo(false));
    }
    
}
