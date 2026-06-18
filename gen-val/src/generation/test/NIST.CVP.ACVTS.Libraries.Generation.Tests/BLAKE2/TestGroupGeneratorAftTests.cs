using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.BLAKE2
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorAftTests
    {
        [Test]
        public async Task ShouldCreateOneGroupPerDigestLength()
        {
            var parameters = new ParameterValidatorTests.ParameterBuilder()
                .WithDigestLengths(256, 512)
                .Build();
            var subject = new TestGroupGeneratorAft();

            var result = await subject.BuildTestGroupsAsync(parameters);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Select(s => s.DigestLength), Is.EquivalentTo(new[] { 256, 512 }));
            Assert.That(result.All(a => a.TestType == "AFT"), Is.True);
            Assert.That(result.All(a => a.HashFunction.Variant == Blake2Variant.Blake2b), Is.True);
        }
    }
}
