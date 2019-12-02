using NIST.CVP.Generation.EDDSA.v1_0.KeyVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "EDDSA",
                Mode = "KeyVer",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "EDDSA",
                Mode = "KeyVer",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
            };

            Assert.AreEqual("EDDSA", parameters.Algorithm);
        }
    }
}
