using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NUnit.Framework;
using System;
using System.Linq;

namespace NIST.CVP.Generation.AES_CBC_CTS.Tests
{
    [TestFixture]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGeneratorKnownAnswerTestsSingleBlock))]
        [TestCase(typeof(TestGroupGeneratorMultiBlockMessageFullBlock))]
        [TestCase(typeof(TestGroupGeneratorMonteCarlo))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainFiveGeneratorsForFullDomain()
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.AreEqual(5, result.Count());
        }
    }
}
