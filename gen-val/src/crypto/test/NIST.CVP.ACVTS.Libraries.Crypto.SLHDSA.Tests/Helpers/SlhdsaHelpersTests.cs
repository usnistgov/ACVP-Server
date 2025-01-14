using System;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.SLH.Tests.Helpers;

[TestFixture, FastCryptoTest]
public class SlhdsaHelpersTests
{

    private readonly IShaFactory _shaFactory = new NativeShaFactory();
    
    [Test]
    [TestCase("n is > 8",new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 9)]
    [TestCase("x.Length is > 8",new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 8)]
    [TestCase("x.Length and n are > 8",new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 9)]
    public void ShouldFailWithBadParametersToInt(string reason, byte[] x, int n)
    {
        Assert.Throws<ArgumentException>(() => SlhdsaHelpers.ToInt(x, n));
    }
    
    [Test]
    [TestCase(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01},8, 1ul)]
    [TestCase(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF},8, 255ul)]
    [TestCase(new byte[] {0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0},8, 1311768467463790320ul)]
    [TestCase(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF},8, 18446744073709551615ul)]
    public void ShouldToIntCorrectly(byte[] x, int n, ulong expected)
    {
        Assert.That(SlhdsaHelpers.ToInt(x, n), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase(0, 32, new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00})]
    [TestCase(84148999, 4, new byte[] {0x05, 0x04, 0x03, 0x07})]
    public void ShouldToByteCorrectly(int x, int n, byte[] expected)
    {
        Assert.That(SlhdsaHelpers.ToByte((ulong)x, n), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(new byte[] {0x00}, 9, 1)]
    public void ShouldFailWithBadParametersBase2B(byte[] x, int b, int outLen)
    {
        Assert.Throws<ArgumentException>(() => SlhdsaHelpers.Base2B(x, b, outLen));
    }
    
    [Test]
    [TestCase(new byte[] {0xF0, 0x31}, 4, 3, new int[] {15, 0, 3})]
    [TestCase(new byte[] {0xF0,0xF0,0xF0,0xF0}, 4, 8, new int[] { 15, 0, 15, 0, 15, 0, 15, 0})]
    [TestCase(new byte[] {0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0,0xF0}, 4, 32, new int[] { 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0, 15, 0})]
    public void ShouldBase2BCorrectly(byte[] x, int b, int outLen, int[] expected)
    {
        var base2BOut = SlhdsaHelpers.Base2B(x, b, outLen); 
        Assert.That(base2BOut, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("i < 0", -1, 0)]
    [TestCase("i > W - 1", 16, 0)]
    [TestCase("s < 0", 0, -1)]
    [TestCase("s > W - 1", 0, 16)]
    [TestCase("(i + s) >= W", 10, 10)]
    public void ShouldFailWithBadParametersChain(string label, int i, int s)
    {
        var slhdsaParameterSetAttribute =
            AttributesHelper.GetParameterSetAttribute(SlhdsaParameterSet.SLH_DSA_SHA2_128f);
        byte[] x = new byte[slhdsaParameterSetAttribute.N];
        byte[] pkSeed = new byte[slhdsaParameterSetAttribute.N];
        var adrs = new WotsHashAdrs(new byte[] { 0x22, 0x22, 0x22, 0x22 }, new byte[] { 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44 });
        var fHOrTFactory = new FHOrTFactory(_shaFactory); 
        var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttribute, FHOrTType.F);

        //SlhdsaHelpers.Chain(x, i, s, pkSeed, adrs, slhdsaParameterSetAttribute, f);
        Assert.Throws<ArgumentException>( () => SlhdsaHelpers.Chain(x, i, s, pkSeed, adrs, slhdsaParameterSetAttribute, f));
    }

    //[Test]
    //public void ShouldSucceedWithValidParametersChain();

}
