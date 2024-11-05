using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyGen
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
                Mode = "keyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ECDSA",
                Mode = "keyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            Assert.That(parameters.Algorithm, Is.EqualTo("ECDSA"));
        }
    }
}
