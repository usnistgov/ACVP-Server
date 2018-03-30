using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CFB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-CFB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assume.That(parameters != null);
            Assert.AreEqual("TDES-CFB", parameters.Algorithm);
        }
    }
}
