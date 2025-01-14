using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Tests;

[TestFixture, FastCryptoTest]
public class PrivateKeyTests
{
    [Test]
    [TestCase(new byte[] {0, 1, 2}, new byte[] {3, 4, 5}, new byte[] {6, 7, 8}, new byte[] {9, 10, 11})]
    public void ShouldConvertConcatenatedKeyIntoParts(byte[] k1, byte[] k2, byte[] k3, byte[] k4)
    {
        var fullK = k1.Concatenate(k2).Concatenate(k3).Concatenate(k4);
        var privateKey = new PrivateKey(fullK);
        
        Assert.That(k1, Is.EqualTo(privateKey.SkSeed));
        Assert.That(k2, Is.EqualTo(privateKey.SkPrf));
        Assert.That(k3, Is.EqualTo(privateKey.PkSeed));
        Assert.That(k4, Is.EqualTo(privateKey.PkRoot));
    }

    [Test]
    [TestCase(new byte[] {0, 1, 2}, new byte[] {3, 4, 5}, new byte[] {6, 7, 8}, new byte[] {9, 10, 11})]
    public void ShouldConvertPartsIntoConcatenatedKey(byte[] k1, byte[] k2, byte[] k3, byte[] k4)
    {
        var fullK = k1.Concatenate(k2).Concatenate(k3).Concatenate(k4);
        var privateKey = new PrivateKey(k1, k2, k3, k4);
        
        Assert.That(fullK, Is.EqualTo(privateKey.GetBytes()));
    }
}
