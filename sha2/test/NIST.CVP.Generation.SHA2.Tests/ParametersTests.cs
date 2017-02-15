using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters {Algorithm = "SHA", Mode = new[] {"SHA1", "SHA2"}, DigestSize = new[] { "160, 512" }, IsSample = false};
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters {Algorithm = "SHA", Mode = new[] {"SHA1, SHA2"}, DigestSize = new [] {"160, 512"}, IsSample = false};
            Assume.That(parameters != null);
            Assert.AreEqual("SHA", parameters.Algorithm);
        }
    }
}
