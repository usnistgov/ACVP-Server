using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
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
            var result = _subject.GetTestGroupGenerators();

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators();

            Assert.IsTrue(result.Count() == 2);
        }

        [Test]
        public void ShouldReturnVectorSet()
        {
            var result = _subject.GetTestGroupGenerators();
            Parameters p = new Parameters
            {
                Algorithm = "SHA1",
                DigestSizes = new[] {"160"},
                BitOriented = true,
                IncludeNull = true,
            };
            
            List<ITestGroup> groups = new List<ITestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.IsNotNull(groups);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators();
            Parameters p = new Parameters
            {
                Algorithm = "SHA2",
                DigestSizes = new[] {"224", "256", "384", "512", "512/224", "512/256"},
                BitOriented = true,
                IncludeNull = true,
            };

            List<ITestGroup> groups = new List<ITestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(12, groups.Count);       // 2 * 6 (aft + mct X digest sizes)
        }
    }
}
