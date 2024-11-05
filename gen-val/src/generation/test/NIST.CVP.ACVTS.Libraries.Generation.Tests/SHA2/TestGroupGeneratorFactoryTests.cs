using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA2
{
    [TestFixture]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorFactory();
        }

        [Test]
        [TestCase(typeof(TestGroupGeneratorAlgorithmFunctionalTest))]
        [TestCase(typeof(TestGroupGeneratorMonteCarloTest))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count() == 3, Is.True);
        }

        [Test]
        public async Task ShouldReturnVectorSet()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Parameters p = new Parameters
            {
                Algorithm = "SHA-1",
                DigestSizes = new List<string>() { "160" },
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 1024, 8))
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.That(groups, Is.Not.Null);
        }

        [Test]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Parameters p = new Parameters
            {
                Algorithm = "SHA2-224",
                DigestSizes = new List<string>() { "224", "256", "384", "512", "512/224", "512/256" },
                PerformLargeDataTest = new[] { 4 },
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 1024, 8))
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.That(groups.Count, Is.EqualTo(18));       // 3 * 6 (aft + mct + ldt * digest sizes)
        }
    }
}
