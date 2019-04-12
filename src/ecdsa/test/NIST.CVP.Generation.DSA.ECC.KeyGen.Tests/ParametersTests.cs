using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
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
                Mode = "KeyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ECDSA",
                Mode = "KeyGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            Assert.AreEqual("ECDSA", parameters.Algorithm);
        }
    }
}
