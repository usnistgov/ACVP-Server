using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX943
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0,  // Hash algo too small to make any valid groups
                new ParameterBuilder()
                    .WithFieldSize(new [] {256})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-224"})
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithFieldSize(new [] {256})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-256"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithFieldSize(new [] {256, 571})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(256)))
                    .WithHashAlg(new [] {"sha2-256", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                24,
                new ParameterBuilder()
                    .WithFieldSize(new [] {283, 521})
                    .WithKeyDataLength(new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.KEY_LENGTH_MINIMUM)).AddSegment(new ValueDomainSegment(ParameterValidator.KEY_LENGTH_MAXIMUM)))
                    .WithSharedInfoLength(new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.SHARED_INFO_MINIMUM)).AddSegment(new ValueDomainSegment(ParameterValidator.SHARED_INFO_MAXIMUM)))
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombination(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}
