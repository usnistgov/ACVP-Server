using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Linq;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGenerator))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            _subject = new TestGroupGeneratorFactory();
            var param = new Parameters();

            var result = _subject.GetTestGroupGenerators(param);

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            _subject = new TestGroupGeneratorFactory();
            var param = new Parameters();

            var result = _subject.GetTestGroupGenerators(param);

            Assert.IsTrue(result.Count() == 1);
        }
    }
}