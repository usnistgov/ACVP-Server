using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.TDES_OFB.v1_0;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFB.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters { Algorithm = "TDES-OFB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-OFB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assume.That(parameters != null);
            Assert.AreEqual("TDES-OFB", parameters.Algorithm);
        }
    }
}
