using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class ForsTests
{
    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    private Fors _subject;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _subject = new Fors(_shaFactory);
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 0, 0, 0, "519B597ADB08365E30D9A5B4FECEA7FE")] // per Section 7, there are 2^h-h' trees on layer 0 of the hypertree
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 1, 511, 0, "4AE62F8359EFCAB6BC2510F4F97C797C")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 8, 0,  0, "4EF600C4C974D3B9B672FED9510EC0A9")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 16,7,  2111, "58691051AC33E93731D117ED0C973E5E")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 32,15,  1, "FA5CC7C7116CFA395785A6BCAC3A4C9DBB5B10DB3A3C274E")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 1024, 256, 1024, "DC385405B73DA65560E986AAB1014E81182EAEA7188F3FE1")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 2048, 2, 10, "9734B1723D594BFDDD84763D1079B18D25B86C5E4AAEFFC5")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 1048576, 5,  100, "8EC302DACE8D41F14C50EA402F27D924B4D3839B2110C6CF")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 0, 0, 2000, "624068823AD70DA123DB59B5ED0244CD8756F8AE55BEDC7F2878EE21630054FA")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 1, 255,  3, "7663B82A17508FF3131D490A81157D92580A50CA5467A1E34306B27FCAA5531A")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 2,0, 4, "23E3C643F5E051906C697F0BE1EB1250A32B8C7A2C35A07A5635FD70AD46192C")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 4,15,  5, "9637E88ECA84D7DDC02C98A51BD8D578F2D03FF72F4D0AECFE21522D41E92ABB")]
    public void ShouldForsSkGenKat(SlhdsaParameterSet slhdsaParameterSet, int treeAdrs, int keyPairAdrs, int forsTreeIdx, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int i = 0; i < slhdsaParameterSetAttributes.N; i++)
        {
            skSeed[i] = 0x1f;
            pkSeed[i] = 0x2e;
        }
        
        // build the address
        var adrs = new ForsTreeAdrs(SlhdsaHelpers.ToByte((ulong)treeAdrs, 12));
        SlhdsaHelpers.ToByte((ulong)keyPairAdrs, adrs.KeyPairAddress.Length).CopyTo(adrs.KeyPairAddress, 0);

        // calculate the FORS secret key value in question
        var sk = _subject.ForsSkGen(skSeed, pkSeed, adrs, forsTreeIdx, slhdsaParameterSetAttributes);
        
        Assert.That(new BitString(sk).ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase("z > a", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 0, 7)]
    [TestCase("i >= k*2^(a-z)", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 2113, 0)]
    [TestCase("z > a and i >= k*2^(a-z)", SlhdsaParameterSet.SLH_DSA_SHA2_128f, 2113, 7)]
    public void ShouldFailWithBadParametersForsNode(string description, SlhdsaParameterSet slhdsaParameterSet, int i, int z)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int j = 0; j < slhdsaParameterSetAttributes.N; j++)
        {
            skSeed[j] = 0x1f;
            pkSeed[j] = 0x2e;
        }
        
        // build the address
        var adrs = new ForsTreeAdrs(SlhdsaHelpers.ToByte(4, 12));
        SlhdsaHelpers.ToByte(7, adrs.KeyPairAddress.Length).CopyTo(adrs.KeyPairAddress, 0);
        
        Assert.Throws<ArgumentException>(() => _subject.ForsNode(skSeed, pkSeed, i, z,
            adrs, slhdsaParameterSetAttributes));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 0, 0, "3AB56490A4FD0FE18B88DFF856A62644")]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 0, 0,"F53455CF63731D4CC695CD123530A0D8")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 65, 5,"C52D47BD8843A0F4759B23B227301D87")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 1024, 1, "405079DAAEF4EE9672A9CAAC46AEC603")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 512, 2, "9DD6ACAB27CE3732D8BD8A7F6AFD8DF5227763056A1637DF")]  
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 256, 3, "CDD72D5B0003FEA3353B70643E6EDD83352B626B4E6AFCCC")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 128, 4,"C816D7D9B35E44F4C15DC30BE2AFDD59BCE2212A26791DB3")]  
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 64, 5,"21ECC1B203724DF38260B1DF12DAFD87F4C1294841D7C665")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 32, 4, "E17F58D1E82720DF48486A82F21C2657605BC1DC11E886BE1FD76950BDD0DA04")]  
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 16, 3,"79BD788943D6D4A030FB3EB437B17034EE49BDB6D968F14DD920C82F07CB287B")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 8, 2, "511D1538C1ADD3BE2052EAEBE65A687A6ED4E76C3D5713F935C951443F4C8099")] 
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 4, 1, "36C906A20E9627C518A499D463F70D85E2230F0D604ECB5E4F2B9B74EE85259E")]
    public void ShouldForsNodeKat(SlhdsaParameterSet slhdsaParameterSet, int i, int z, string expected)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        
        // build the seeds and message
        for (int j = 0; j < slhdsaParameterSetAttributes.N; j++)
        {
            skSeed[j] = 0x1f;
            pkSeed[j] = 0x2e;
        }
        
        // build the address
        var adrs = new ForsTreeAdrs(SlhdsaHelpers.ToByte(4, 12));
        SlhdsaHelpers.ToByte(7, adrs.KeyPairAddress.Length).CopyTo(adrs.KeyPairAddress, 0);
        
        // calculate the FORS node identified by i and z
        var forsNode = _subject.ForsNode(skSeed, pkSeed, i, z, adrs, slhdsaParameterSetAttributes);
        
        Assert.That(new BitString(forsNode).ToHex(), Is.EqualTo(new BitString(expected).ToHex()));
    }

    [Test]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 0, 0)] // per Section 7, there are 2^h-h' trees on layer 0 of the hypertree
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128s, 1, 511)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 8, 0)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_128f, 16,7)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 32,15)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192s, 1024, 256)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 2048, 2)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_192f, 1048576, 5)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 0, 0)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 1, 255)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 2,0)]
    [TestCase(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 4,15)]
    public void ShouldForsSignAndVerifySuccessfully(SlhdsaParameterSet slhdsaParameterSet, int treeAdrs, int keyPairAdrs)
    {
        // grab all the values associated w/ the selected parameter set
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(slhdsaParameterSet);
        byte[] skSeed = new byte[slhdsaParameterSetAttributes.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttributes.N];
        // per section 8.3 footnote 12, fors_sign takes as input a ceiling(k*a/8) byte message digest
        int mdByteLen = (int) System.Math.Ceiling((double) slhdsaParameterSetAttributes.K * slhdsaParameterSetAttributes.A / 8);
        byte[] md = new byte[mdByteLen];
        
        // build the seeds
        for (int j = 0; j < slhdsaParameterSetAttributes.N; j++)
        {
            skSeed[j] = 0x1f;
            pkSeed[j] = 0x2e;
        }
        
        // build the message digest
        for (int k = 0; k < mdByteLen; k++)
        {
            md[k] = 0xF0;
        }
        
        // build the address
        var adrs = new ForsTreeAdrs(SlhdsaHelpers.ToByte((ulong)treeAdrs, 12));
        SlhdsaHelpers.ToByte((ulong)keyPairAdrs, adrs.KeyPairAddress.Length).CopyTo(adrs.KeyPairAddress, 0);
        
        // In FIPS 205 Section 8.4 it states, "[FORS signature] verification succeeds if the correct public-key value is
        // computed, which is determined by verifying the hypertree signature on the computed public-key value using the
        // SLH-DSA public key."
        // For this test, however, we'll do something more circular. We'll 1) calculate the expected FORS public key value by 
        // hand using ForsNode(), 2) sign a message digest via ForsSign(), 3) pull back the candidate FORS public key value using 
        // ForsPKFromSig() and 4) compare the candidate public key value from #3 against the value we computed by hand in #1.
        
        // 1) calculate the expected FORS public key value by hand using ForsNode()
        byte[] rootNodes = new byte[slhdsaParameterSetAttributes.K * slhdsaParameterSetAttributes.N];
        for (int i = 0; i < slhdsaParameterSetAttributes.K; i++)
        {
            var rootNode = _subject.ForsNode(skSeed, pkSeed, i, slhdsaParameterSetAttributes.A, adrs, slhdsaParameterSetAttributes);
            rootNode.CopyTo(rootNodes, i * slhdsaParameterSetAttributes.N);
        }
        // Compute the FORS public key from the Merkle tree roots
        var fHOrTFactory = new FHOrTFactory(_shaFactory);
        var t = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.T);
        var forsPKAdrs = new ForsRootsAdrs(adrs.TreeAddress);
        forsPKAdrs.KeyPairAddress = adrs.KeyPairAddress;
        var forsPK = t.Hash(pkSeed, forsPKAdrs, rootNodes);
        
        // 2) sign a message digest via ForsSign()
        var sigFors = _subject.ForsSign(md, skSeed, pkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 3) pull back the candidate FORS public key value using ForsPKFromSig()
        var candidateForsPK = _subject.ForsPKFromSig(sigFors, md, pkSeed, adrs, slhdsaParameterSetAttributes);
        
        // 4) compare the candidate public key value from #3 against the value we computed by hand in #1
        Assert.That(new BitString(candidateForsPK).ToHex(), Is.EqualTo(new BitString(forsPK).ToHex()));
    }
}
