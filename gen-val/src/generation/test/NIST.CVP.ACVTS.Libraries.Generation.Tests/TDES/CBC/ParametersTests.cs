using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBC.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBC
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CBC", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CBC", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.That(parameters != null);
            Assert.That(parameters.Algorithm, Is.EqualTo("TDES-CBC"));
        }
    }
}
