using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                PubExpMode = PublicExponentModes.Random,
                IsSample = false,
                InfoGeneratedByServer = false,
                KeyFormat = PrivateKeyModes.Standard,
                AlgSpecs = BuildCapabilities()
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                InfoGeneratedByServer = true,
                IsSample = false,
                PubExpMode = PublicExponentModes.Fixed,
                KeyFormat = PrivateKeyModes.Standard,
                FixedPubExp = new BitString("010001"),
                AlgSpecs = BuildCapabilities()
            };

            Assert.AreEqual("RSA", parameters.Algorithm);
        }

        private AlgSpec[] BuildCapabilities()
        {
            var caps = new Capability[1];
            caps[0] = new Capability
            {
                Modulo = ParameterValidator.VALID_MODULI.First(),
                HashAlgs = ParameterValidator.VALID_HASH_ALGS,
                PrimeTests = new[] { PrimeTestModes.TwoPow100ErrorBound }
            };

            var algSpecs = new []
            {
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes,
                    Capabilities = caps
                } 
            };

            return algSpecs;
        }
    }
}
