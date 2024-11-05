using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CCM
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

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            _subject = new TestGroupGeneratorFactory();
            var param = new Parameters();

            var result = _subject.GetTestGroupGenerators(param);

            Assert.That(result.Count() == 1, Is.True);
        }
    }
}
