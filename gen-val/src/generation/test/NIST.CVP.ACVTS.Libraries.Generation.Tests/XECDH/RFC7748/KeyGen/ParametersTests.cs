using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.KeyGen
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "XECDH",
                Mode = "keyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES
            };

            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "XECDH",
                Mode = "keyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES
            };

            Assert.That(parameters.Algorithm, Is.EqualTo("XECDH"));
        }
    }
}
