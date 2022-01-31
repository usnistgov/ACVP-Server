using NIST.CVP.ACVTS.Libraries.Generation.TDES_ECB.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.ECB
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-ECB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-ECB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.That(parameters != null);
            Assert.AreEqual("TDES-ECB", parameters.Algorithm);
        }
    }
}
