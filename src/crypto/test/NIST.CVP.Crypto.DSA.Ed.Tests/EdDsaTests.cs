using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DSA.Ed.Tests
{
    [TestFixture, LongCryptoTest]
    public class EdDsaTests
    {
        [Test]
        #region KeyPairGen-25519
        [TestCase(Curve.Ed25519,
            "9d61b19deffd5a60ba844af492ec2cc44449c5697b326919703bac031cae7f60",
            "d75a980182b10ab7d54bfed3c964073a0ee172f3daa62325af021a68f707511a",
            TestName = "KeyGen 25519 - 1")]
        #endregion KeyPairGen-25519
        #region KeyPairGen-448
        [TestCase(Curve.Ed448,
            "6c82a562cb808d10d632be89c8513ebf6c929f34ddfa8c9f63c9960ef6e348a3528c8a3fcc2f044e39a3fc5b94492f8f032e7549a20098f95b",
            "5fd7449b59b461fd2ce787ec616ad46a1da1342485a70e1f8a0ea75d80e96778edf124769b46c7061bd6783df1e50f6cd1fa1abeafe8256180",
            TestName = "KeyGen 448 - 1")]
        #endregion KeyPairGen-448
        public void ShouldGenerateKeyPairsCorrectly(Curve curveEnum, string dHex, string qHex)
        {
            var d = LoadValue(dHex);
            var q = LoadValue(qHex);

            var factory = new EdwardsCurveFactory();
            var curve = factory.GetCurve(curveEnum);

            var domainParams = new EdDomainParameters(curve);

            var subject = new EdDsa(EntropyProviderTypes.Testable);
            subject.AddEntropy(d);

            var result = subject.GenerateKeyPair(domainParams);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.KeyPair.PrivateD, d, "d");
            Assert.AreEqual(q, result.KeyPair.PublicQ, "q");
        }

        private BigInteger LoadValue(string hex)
        {
            return new BitString(hex).ToPositiveBigInteger();
        }
    }
}
