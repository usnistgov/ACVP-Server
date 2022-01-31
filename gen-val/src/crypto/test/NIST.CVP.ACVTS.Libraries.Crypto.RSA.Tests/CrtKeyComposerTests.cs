using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests
{
    [TestFixture, FastCryptoTest]
    public class CrtKeyComposerTests
    {
        [Test]
        [TestCase("11", "3D", "35", "0CA1", "35", "31", "26")]
        public void ShouldComposeCrtKeyCorrectly(string eHex, string pHex, string qHex, string nHex, string dmp1Hex, string dmq1Hex, string iqmpHex)
        {
            var e = new BitString(eHex).ToPositiveBigInteger();
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var expectedN = new BitString(nHex).ToPositiveBigInteger();
            var expectedDmp1 = new BitString(dmp1Hex).ToPositiveBigInteger();
            var expectedDmq1 = new BitString(dmq1Hex).ToPositiveBigInteger();
            var expectedIqmp = new BitString(iqmpHex).ToPositiveBigInteger();

            var subject = new CrtKeyComposer();
            var result = subject.ComposeKey(e, new PrimePair { P = p, Q = q });

            Assert.AreEqual(expectedN, result.PubKey.N, "n");
            Assert.AreEqual(expectedDmp1, ((CrtPrivateKey)result.PrivKey).DMP1, "dmp1");
            Assert.AreEqual(expectedDmq1, ((CrtPrivateKey)result.PrivKey).DMQ1, "dmq1");
            Assert.AreEqual(expectedIqmp, ((CrtPrivateKey)result.PrivKey).IQMP, "iqmp");
        }
    }
}
