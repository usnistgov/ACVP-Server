﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE
{
    [TestFixture, UnitTest]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "cSHAKE",
                DigestSizes = new List<int>(),
                HexCustomization = false,
                MessageLength = new MathDomain(),
                OutputLength = new MathDomain(),
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
                Algorithm = "cSHAKE",
                DigestSizes = new List<int>(),
                HexCustomization = false,
                MessageLength = minMax,
                OutputLength = minMax,
                IsSample = false
            };

            Assert.That(parameters != null);
            Assert.That(parameters.Algorithm, Is.EqualTo("cSHAKE"));
        }
    }
}
