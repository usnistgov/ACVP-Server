﻿using NIST.CVP.Generation.SHA3.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
                DigestSizes = new [] {224, 256, 384, 512},
                BitOrientedInput = true,
                IncludeNull = true,
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
                Algorithm = "SHAKE",
                DigestSizes = new [] {128, 256},
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                OutputLength = minMax,
                IsSample = false
            };

            Assume.That(parameters != null);
            Assert.AreEqual("SHAKE", parameters.Algorithm);
        }
    }
}
