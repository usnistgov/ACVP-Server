using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.SigGen
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ECDSA",
                Mode = "sigGen",
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
                Algorithm = "ECDSA",
                Mode = "sigGen",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.That(parameters.Algorithm, Is.EqualTo("ECDSA"));
        }

        private Capability[] GetCapabilities()
        {
            return new Capability[]
            {
                new Capability
                {
                    Curve = ParameterValidator.VALID_CURVES,
                    HashAlg = ParameterValidator.VALID_HASH_ALGS
                }
            };
        }
    }
}
