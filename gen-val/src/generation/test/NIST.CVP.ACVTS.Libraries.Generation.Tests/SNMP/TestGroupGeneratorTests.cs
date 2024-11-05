using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SNMP
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd"})
                    .WithPasswordLength(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd"})
                    .WithPasswordLength(new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 1024)))
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithEngineId(new [] {"abcdabcdabcdabcdabcd", "abcdabcdabcdabcdef"})
                    .WithPasswordLength(new MathDomain().AddSegment(new ValueDomainSegment(128)).AddSegment(new ValueDomainSegment(512)))
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfVersionAndHash(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}
