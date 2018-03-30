using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters {Algorithm = "TDES-ECB", Direction = new[] {"Encrypt"}, IsSample = false};
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters { Algorithm = "TDES-ECB", Direction = new[] { "Encrypt" }, IsSample = false };
            Assume.That(parameters != null);
            Assert.AreEqual("TDES-ECB", parameters.Algorithm);
        }
    }
}
