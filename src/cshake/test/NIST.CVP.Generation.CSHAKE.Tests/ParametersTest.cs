using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.CSHAKE.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CSHAKE.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "CSHAKE",
                DigestSizes = new int[5],
                HexCustomization = false,
                MessageLength = new MathDomain(),
                OutputLength = new MathDomain(),
                IsSample = false
            };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            var parameters = new Parameters
            {
                Algorithm = "CSHAKE",
                DigestSizes = new int[5],
                HexCustomization = false,
                MessageLength = minMax,
                OutputLength = minMax,
                IsSample = false
            };

            Assume.That(parameters != null);
            Assert.AreEqual("CSHAKE", parameters.Algorithm);
        }
    }
}
