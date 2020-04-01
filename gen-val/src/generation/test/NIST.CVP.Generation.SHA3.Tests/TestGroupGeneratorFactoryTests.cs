using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.SHA3.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorFactory();
        }

        [Test]
        [TestCase(typeof(TestGroupGeneratorAlgorithmFunctional))]
        [TestCase(typeof(TestGroupGeneratorMonteCarlo))]
        [TestCase(typeof(TestGroupGeneratorVariableOutput))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            Parameters p = new Parameters
            {
                Algorithm = "SHAKE",
                DigestSizes = new[] {128, 256},
                BitOrientedInput = true,
                BitOrientedOutput = true,
                IncludeNull = true,
                IsSample = false,
                OutputLength = minMax
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.AreEqual(6, groups.Count);       // 2 * 3 (digest sizes * test types)
        }
    }
}
