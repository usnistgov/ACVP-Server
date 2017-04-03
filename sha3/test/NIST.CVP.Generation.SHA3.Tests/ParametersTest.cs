using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                Function = new[] { "SHA3", "SHAKE" },
                DigestSize = new[] { 256, 128 },
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                MinOutputLength = 16,
                MaxOutputLength = 65336,
                IsSample = false
            };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA3",
                Function = new[] { "SHA3", "SHAKE" },
                DigestSize = new[] { 256, 128 },
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                MinOutputLength = 16,
                MaxOutputLength = 65336,
                IsSample = false
            };

            Assume.That(parameters != null);
            Assert.AreEqual("SHA3", parameters.Algorithm);
        }
    }
}
