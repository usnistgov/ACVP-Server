using NIST.CVP.Generation.TDES_CBC.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CBC", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CBC", Direction = new[] { "Encrypt" }, IsSample = false };
            Assume.That(parameters != null);
            Assert.AreEqual("TDES-CBC", parameters.Algorithm);
        }
    }
}
