using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFBP
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CFBP", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CFBP", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.That(parameters != null);
            Assert.That(parameters.Algorithm, Is.EqualTo("TDES-CFBP"));
        }
    }
}
