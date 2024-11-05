using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC.Tests
{
    [TestFixture, LongCryptoTest]
    public class DeterministicEccDsaTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, Curve.P192)]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, Curve.P192)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, Curve.P224)]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P192)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P224)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, Curve.P256)]
        public void ShouldVerifyRandomlyGeneratedSignatures(ModeValues mode, DigestSizes digest, Curve curveEnum)
        {
            var nonces = new List<BigInteger>();

            var hashFunction = new HashFunction(mode, digest);
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(hashFunction);
            var hmacFactory = new HmacFactory(shaFactory);
            var hmac = hmacFactory.GetHmacInstance(hashFunction);

            var subject = new EccDsa(sha, new DeterministicNonceProvider(hmac), EntropyProviderTypes.Random);

            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(curveEnum);
            var domainParams = new EccDomainParameters(curve);
            var key = subject.GenerateKeyPair(domainParams).KeyPair;

            var rand = new Random800_90();

            for (var i = 0; i < 100; i++)
            {
                var message = rand.GetRandomBitString(1024);

                var signature = subject.Sign(domainParams, key, message).Signature;
                var verify = subject.Verify(domainParams, key, message, signature);

                nonces.Add(signature.R);

                Assert.That(verify.Success, Is.True, verify.ErrorMessage);
            }

            // Check nonces for uniqueness
            Assert.That(nonces.Distinct().Count(), Is.EqualTo(nonces.Count), "Repeated nonce detected");
        }
    }
}
