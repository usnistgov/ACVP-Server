using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ECDSA",
                Mode = "SigGen",
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
                Algorithm = "ECDSA",
                Mode = "SigGen",
                IsSample = false,
                Capabilities = GetCapabilities()
            };

            Assert.AreEqual("ECDSA", parameters.Algorithm);
        }

        private Capability[] GetCapabilities()
        {
            return new Capability[]
            {
                new Capability
                {
                    Curve = ParameterValidator.VALID_CURVES,
                    HashAlg = ParameterValidator.VALID_HASH_ALGS
                }
            };
        }
    }
}
