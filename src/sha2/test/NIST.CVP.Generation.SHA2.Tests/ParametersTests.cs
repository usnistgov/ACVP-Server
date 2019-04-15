using NIST.CVP.Generation.SHA2.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA1",
                DigestSizes = new [] {"160"},
                IsSample = false,
                MessageLength = new MathDomain()
            };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA2",
                DigestSizes = new [] {"224", "256", "384", "512", "512/224", "512/256"},
                IsSample = false,
                MessageLength = new MathDomain()
            };
            Assert.AreEqual("SHA2", parameters.Algorithm);
        }
    }
}
