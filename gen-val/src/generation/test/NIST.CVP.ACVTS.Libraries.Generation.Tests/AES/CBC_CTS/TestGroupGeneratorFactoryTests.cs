using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CBC_CTS
{
    [TestFixture]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGeneratorKnownAnswerTestsSingleBlock))]
        [TestCase(typeof(TestGroupGeneratorMultiBlockMessageFullBlock))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainFourGeneratorsForFullDomain()
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.That(result.Count(), Is.EqualTo(4));
        }
    }
}
