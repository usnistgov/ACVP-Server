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
                Algorithm = "SHA",
                Functions = new []
                {
                    new Function
                    {
                        Mode = "sha1",
                        DigestSizes = new [] {"160"}
                    },
                    new Function
                    {
                        Mode = "sha2",
                        DigestSizes = new [] {"512"}
                    }
                },
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
                Algorithm = "SHA",
                Functions = new[]
                {
                    new Function
                    {
                        Mode = "sha1",
                        DigestSizes = new [] {"160"}
                    },
                    new Function
                    {
                        Mode = "sha2",
                        DigestSizes = new [] {"512"}
                    }
                },
                IsSample = false,
                BitOriented = false,
                IncludeNull = false
            };
            Assert.AreEqual("SHA", parameters.Algorithm);
        }
    }
}
