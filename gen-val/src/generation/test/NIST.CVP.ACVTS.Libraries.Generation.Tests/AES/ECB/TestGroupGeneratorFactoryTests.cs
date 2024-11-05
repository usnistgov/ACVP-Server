using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.ECB
{
    [TestFixture]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGeneratorKnownAnswerTests))]
        [TestCase(typeof(TestGroupGeneratorMultiBlockMessage))]
        [TestCase(typeof(TestGroupGeneratorMonteCarlo))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count() == 3, Is.True);
        }
    }
}
