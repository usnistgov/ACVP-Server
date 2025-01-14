using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class XmssTests
{
    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    private IWots _wots;
    private Xmss _subject;

    [OneTimeSetUp]
    public void Setup()
    {
        _wots = new Wots(_shaFactory);
        _subject = new Xmss(_shaFactory, _wots);
    }
    
    [Test]
    [TestCase("z > h′", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 0, 4)]
    [TestCase("i >= 2^(h′-z)", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 8, 0)]
    [TestCase("z > h′ and i >= 2^(h′-z)", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 8, 4)]
    public void ShouldFailWithBadParametersXmssNode(string description, SlhdsaParameterSet slhdsaParameterSet, int i, int z)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        
        Assert.Throws<ArgumentException>(() => _subject.XmssNode(skSeed, pkSeed, i, z,
            adrs, slhdsaParameterSetAttributes));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, "31CAB6779DE5F6D1FF181262E69B9F3E")] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, "388275A1BF846E290A4ED8A14906F0AF")] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, "9BA9D8B626E22C6EAAFE889D1E2434C4")] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, "005C481F0D17AEE3D4301C0B43E85049")] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, "198A7171F340089142AD350C55F176B990EEC48B032FFBF8")] // hPrime = 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, "86BB3FAD8040506CEC9DD5C75B841867018F938590094F81")] // hPrime = 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, "73A9427B9AC30638DB84CAE6FB5EBC6D7B398C31675E0381")] // hPrime = 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, "E7365962E07C00EBB7717EA564110E9B7B897DEA6303CE50")] // hPrime = 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, "A2F3F360D0BBA47C1A6E116ACB2F3D13C5F7B482C0314705AEA5A1DA2F1C8EE8")]  // hPrime = 8
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, "8606FA554C1E1CD578CC676B76CF2B567652B41376E1614143521C85D83ED8C6")]  // hPrime = 8
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, "DB971CB89C00E021AEC9D3B852A145091A5DBD8C8610B58B5116B396AF4C97EA")] // hPrime = 4
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, "C65C121C1B844194A9B7932C4C8D5AD9E90EAE2D7C3ADFBA77472C0BBE2341D9")] // hPrime = 4
    public void ShouldXmssNodeKat(SlhdsaParameterSet slhdsaParameterSet, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        // NOTE: these layer and tree addresses may not be valid. Probably worth investigating at some point...
        // i.e., it might make sense to test using "valid" layer and tree addresses
        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        
        // build the seeds
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            skSeed[i] = 0x1f;
            pkSeed[i] = 0x2e;
        }

        // calculate the root node for the XMSS tree in question
        var xmssNode = _subject.XmssNode(skSeed, pkSeed, 0, slhdsaParameterSetAttributes.HPrime, adrs,
            slhdsaParameterSetAttributes);
        
        Assert.That(new BitString(xmssNode).ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 0)] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 511)] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 0)] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 7)] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 15)] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 256)] // hPrime = 9
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 2)] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 5)] // hPrime = 3
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 0)] // hPrime = 8
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 255)] // hPrime = 8
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 0)] // hPrime = 4
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 15)] // hPrime = 4
    public void ShouldSignAndVerifySuccessfully(SlhdsaParameterSet slhdsaParameterSet, int idx)
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
        // NOTE: these layer and tree addresses may not be valid. Probably worth investigating at some point...
        // i.e., it might make sense to test using "valid" layer and tree addresses
        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        
        // 1) generate/calculate the root node value for the XMSS tree identified by adrs
        var xmssNode = _subject.XmssNode(skSeed, pkSeed, 0, slhdsaParameterSetAttributes.HPrime, adrs,
            slhdsaParameterSetAttributes);
        
        // 2) sign message using the WOTS+ key identified by idx
        var signature = _subject.XmssSign(message, skSeed, pkSeed, idx, adrs, slhdsaParameterSetAttributes);
        
        // 3) calculate a candidate XMSS root node value for the XMSS tree identified by adrs given the message and its signature
        var candidateXmssNode =
            _subject.XmssPKFromSig(idx, signature, message, pkSeed, adrs, slhdsaParameterSetAttributes);

        // 4) candidateXmssNode should be equal to/match xmssNode
        Assert.That(new BitString(candidateXmssNode).ToHex(), Is.EqualTo(new BitString(xmssNode).ToHex()));
    }
}
