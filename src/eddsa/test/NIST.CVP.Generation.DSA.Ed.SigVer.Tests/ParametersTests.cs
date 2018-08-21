using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "EDDSA",
                Mode = "SigVer",
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
                Algorithm = "EDDSA",
                Mode = "SigVer",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.AreEqual("EDDSA", parameters.Algorithm);
        }

        private Capability[] GetCapabilities()
        {
            return new Capability[]
            {
                new Capability
                {
                    Curve = ParameterValidator.VALID_CURVES
                }
            };
        }
    }
}
