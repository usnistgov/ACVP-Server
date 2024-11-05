using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.AES
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

            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            _subject = new TestGroupGeneratorFactory();

            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count() == 1, Is.True);
        }
    }
}
