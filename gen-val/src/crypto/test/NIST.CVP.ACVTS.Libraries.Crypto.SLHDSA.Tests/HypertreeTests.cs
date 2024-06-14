using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class HypertreeTests
{
    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    private IWots _wots;
    private IXmss _xmss;
    private Hypertree _subject;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _wots = new Wots(_shaFactory);
        _xmss = new Xmss(_shaFactory, _wots);
        _subject = new Hypertree(_xmss);
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, "E6B327D2DE28658C7FC53A5B544A45C0")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, "CBBA47776E0E9335202420FA845F0640")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, "E6B327D2DE28658C7FC53A5B544A45C0")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, "CBBA47776E0E9335202420FA845F0640")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, "1595D08E1532E575DF963D246F3DBF04")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, "7FF0B394D7E4E1B91B96F29DE7C9A5C3")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, "1595D08E1532E575DF963D246F3DBF04")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, "7FF0B394D7E4E1B91B96F29DE7C9A5C3")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, "B41730DF6A705881149422897DC78A19")]  
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, "A52AB0EDD0D71A136E702191148C23EB")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, "B41730DF6A705881149422897DC78A19")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, "A52AB0EDD0D71A136E702191148C23EB")] 
    public void ShouldHypertreeSignKat(SlhdsaParameterSet slhdsaParameterSet, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] message = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            skSeed[i] = 0x1f;
            pkSeed[i] = 0x2e;
            message[i] = 0xF0;
        }

        // calculate the hypertree signature of message
        var sigHt = _subject.HypertreeSign(message, skSeed, pkSeed, 0, 0, slhdsaParameterSetAttributes);
        // hypertree signatures are large; only compare the first 16 bytes of the signature
        Assert.That(new BitString(sigHt[..16]).ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 0, 0ul, true)] // per Section 7, there are 2^h-h' trees on layer 0 of the hypertree
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 511, 1ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 0, 8ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 7, 16ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 15, 32ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 256, 1024ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 2, 2048ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 5, 1048576ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 0, 0ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 255, 1ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 0, 2ul, true)] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 15, 4ul, true)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 15, 4ul, false)]
    public void ShouldSignAndVerifyAsExpected(SlhdsaParameterSet slhdsaParameterSet, int idxLeaf, ulong idxTree, bool shouldVerifySuccessfully)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] message = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            skSeed[i] = 0x1f;
            pkSeed[i] = 0x2e;
            message[i] = 0xF0;
        }
        
        // 1) calculate the value of PK.root for the hypertree, i.e., the root node of the XMSS tree at the top layer of the hypertree
        // layer and tree addresses will default to 0; we want a layer address of d - 1 and a tree address of 0 
        var adrs = new WotsHashAdrs();
        adrs.LayerAddress = SlhdsaHelpers.ToByte((ulong)slhdsaParameterSetAttributes.D - 1, adrs.LayerAddress.Length);
        var pkRoot = _xmss.XmssNode(skSeed, pkSeed, 0, slhdsaParameterSetAttributes.HPrime, adrs,
            slhdsaParameterSetAttributes);
        
        // 2) calculate the hypertree signature of message
        var sigHt = _subject.HypertreeSign(message, skSeed, pkSeed, idxTree, idxLeaf, slhdsaParameterSetAttributes);

        // 3) do we want the verify to fail? If so, alter the message.
        if (!shouldVerifySuccessfully)
        {
            message[0] = 0xEE;
        }
        
        // 4) verify the hypertree signature
        var result = _subject.HypertreeVerify(message, sigHt, pkSeed, idxTree, idxLeaf, pkRoot,
            slhdsaParameterSetAttributes);
        
        Assert.That(result, Is.EqualTo(shouldVerifySuccessfully));
    }
}
