using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "DSA",
                Mode = "KeyGen",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "DSA",
                Mode = "KeyGen",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.AreEqual("DSA", parameters.Algorithm);
        }

        private Capability[] GetCapabilities()
        {
            var caps = new Capability[3];
            caps[0] = new Capability
            {
                L = 2048,
                N = 224
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256
            };

            return caps;
        }
    }
}
