using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGVer
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "DSA",
                Mode = "pqgVer",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "DSA",
                Mode = "pqgVer",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.That(parameters.Algorithm, Is.EqualTo("DSA"));
        }

        private Capability[] GetCapabilities()
        {
            var caps = new Capability[3];
            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            return caps;
        }
    }
}
