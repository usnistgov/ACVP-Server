using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new [] {128, 256},
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                MinOutputLength = 16,
                MaxOutputLength = 65336,
                IsSample = false
            };

            Assume.That(parameters != null);
            Assert.AreEqual("SHAKE", parameters.Algorithm);
        }
    }
}

