using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA2
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA1",
                DigestSizes = new List<string>() { "160" },
                IsSample = false,
                MessageLength = new MathDomain()
            };
            Assert.That(parameters, Is.Not.Null);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "SHA2",
                DigestSizes = new List<string>() { "224", "256", "384", "512", "512/224", "512/256" },
                IsSample = false,
                MessageLength = new MathDomain()
            };
            Assert.That(parameters.Algorithm, Is.EqualTo("SHA2"));
        }
    }
}
