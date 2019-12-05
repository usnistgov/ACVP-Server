using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DSA.ECC.Tests
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
            var shaFactory = new ShaFactory();
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
            
                Assert.IsTrue(verify.Success, verify.ErrorMessage);    
            }
            
            // Check nonces for uniqueness
            Assert.AreEqual(nonces.Count, nonces.Distinct().Count(), "Repeated nonce detected");
        }
    }
}