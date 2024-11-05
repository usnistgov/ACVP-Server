using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v1_0
{
    [TestFixture, UnitTest]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                DigestSizes = new List<int>() { 224, 256, 384, 512 },
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = false
            };
            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new List<int>() { 128, 256 },
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                OutputLength = minMax,
                IsSample = false
            };

            Assert.That(parameters != null);
            Assert.That(parameters.Algorithm, Is.EqualTo("SHAKE"));
        }
    }
}

