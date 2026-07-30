using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.BLAKE2
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorAftTests
    {
        [Test]
        public async Task ShouldCreateOneGroupWithDigestLengthDomain()
        {
            var parameters = new ParameterValidatorTests.ParameterBuilder()
                .Build();
            var subject = new TestGroupGeneratorAft();

            var result = await subject.BuildTestGroupsAsync(parameters);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].DigestLength.GetDomainMinMax().Minimum, Is.EqualTo(256));
            Assert.That(result[0].DigestLength.GetDomainMinMax().Maximum, Is.EqualTo(512));
            Assert.That(result.All(a => a.TestType == "AFT"), Is.True);
        }
    }
}
