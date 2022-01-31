using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigGen
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "LMS",
                Mode = "SigGen",
                IsSample = false
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "LMS",
                Mode = "SigGen",
                IsSample = false
            };

            Assert.AreEqual("LMS", parameters.Algorithm);
        }
    }
}
