using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                PubExpMode = "random",
                IsSample = false,
                InfoGeneratedByServer = false,
                KeyFormat = "standard",
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
                PubExpMode = "fixed",
                KeyFormat = "standard",
                FixedPubExp = "010001",
                AlgSpecs = BuildCapabilities()
            };

            Assert.AreEqual("RSA", parameters.Algorithm);
        }

        private AlgSpec[] BuildCapabilities()
        {
            var caps = new Capability[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new Capability
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashAlgs = ParameterValidator.VALID_HASH_ALGS,
                    PrimeTests = ParameterValidator.VALID_PRIME_TESTS
                };
            }

            var algSpecs = new AlgSpec[ParameterValidator.VALID_KEY_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpec
                {
                    RandPQ = ParameterValidator.VALID_KEY_GEN_MODES[i],
                    Capabilities = caps
                };
            }

            return algSpecs;
        }
    }
}
