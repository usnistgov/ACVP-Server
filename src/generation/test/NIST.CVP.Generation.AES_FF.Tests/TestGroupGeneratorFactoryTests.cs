using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NUnit.Framework;
using System;
using System.Linq;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    [TestFixture]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGenerator))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainOneGeneratorsForFullDomain()
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new ParameterBuilder().Build());

            Assert.AreEqual(1, result.Count());
        }
    }
}
