using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class WotsTests
{
    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    private Wots _subject;

    [OneTimeSetUp]
    public void Setup()
    {
        _subject = new Wots(_shaFactory);
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, "B1CC2E5736402B0C7EF212404E76D55B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, "F7CAA4A2DF20652F4AF5C609CEFF1920")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, "B1CC2E5736402B0C7EF212404E76D55B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, "F7CAA4A2DF20652F4AF5C609CEFF1920")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, "28CC7C486777785DA4666EC6973FA7D145E488109CACFB9B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, "5CDDF1AEDF2EC59D096B78050BC6394E679BB18DA7284193")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, "28CC7C486777785DA4666EC6973FA7D145E488109CACFB9B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, "5CDDF1AEDF2EC59D096B78050BC6394E679BB18DA7284193")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, "30FF2C589697C3F27AD00A0663B8F5A96D71BCBADF5EE066FF094196CA88AF9B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, "DC192754652BC3054D5C3EF7568D012A4171AA2428677B4307FFCD94736CBDED")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, "30FF2C589697C3F27AD00A0663B8F5A96D71BCBADF5EE066FF094196CA88AF9B")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, "DC192754652BC3054D5C3EF7568D012A4171AA2428677B4307FFCD94736CBDED")]
    public void ShouldPublicKeyGenKat(SlhdsaParameterSet slhdsaParameterSet, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        
        // build the seeds
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            //skSeed[i] = 31;
            skSeed[i] = 0x1f;
            //pkSeed[i] = 46;
            pkSeed[i] = 0x2e;
        }
        
        // generate the WOTS+ public key
        var publicKey = _subject.WotsPKGen(skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);

        Assert.That(new BitString(publicKey).ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f)]
    public void WhenGivenSeedsAddressAndMessage_ShouldSignAndVerifySuccessfully(SlhdsaParameterSet slhdsaParameterSet)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] message = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            //skSeed[i] = 31;
            skSeed[i] = 0x1f;
            //pkSeed[i] = 46;
            pkSeed[i] = 0x2e;
            //message[i] = 240;
            message[i] = 0xF0;
        }

        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        
        // 1) generate the WOTS+ public key associated w/ adrs
        var publicKey = _subject.WotsPKGen(skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);

        // 2) sign message (using the WOTS+ private key associated w/ adrs)
        var signature = _subject.WotsSign(message, skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 3) calculate a candidate WOTS+ public key for adrs given message and its signature (produced using the WOTS+ private key assoc. w/ adrs)
        var candidatePublicKey = _subject.WotsPKFromSig(signature, message, pkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 4) candidatePublicKey should be equal to/match publicKey
        Assert.That(new BitString(candidatePublicKey).ToHex(), Is.EqualTo(new BitString(publicKey).ToHex()));
    }
}
