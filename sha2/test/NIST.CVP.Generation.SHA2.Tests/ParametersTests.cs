using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
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
                BitOriented = false,
                IncludeNull = false
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
                BitOriented = false,
                IncludeNull = false
            };
            Assert.AreEqual("SHA2", parameters.Algorithm);
        }
    }
}
