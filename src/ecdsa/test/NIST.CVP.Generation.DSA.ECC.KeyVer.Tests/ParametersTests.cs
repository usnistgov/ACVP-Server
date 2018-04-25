using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
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
                Mode = "KeyVer",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ECDSA",
                Mode = "KeyVer",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
            };

            Assert.AreEqual("ECDSA", parameters.Algorithm);
        }
    }
}
