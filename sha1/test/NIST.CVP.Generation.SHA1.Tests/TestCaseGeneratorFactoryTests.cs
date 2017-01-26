using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("MMT", typeof(TestCaseGeneratorMMTHash))]
        [TestCase("Mmt", typeof(TestCaseGeneratorMMTHash))]
        [TestCase("mmt", typeof(TestCaseGeneratorMMTHash))]
        [TestCase("MCT", typeof(TestCaseGeneratorMCTHash))]
        [TestCase("Mct", typeof(TestCaseGeneratorMCTHash))]
        [TestCase("mct", typeof(TestCaseGeneratorMCTHash))]
        [TestCase("Junk", typeof(TestCaseGeneratorNull))]
        [TestCase("", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType
            };

            var subject = new TestCaseGeneratorFactory(null, null, null);
            var result = subject.GetCaseGenerator(testGroup);

            Assume.That(result != null);
            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null, null, null);
            var generator = subject.GetCaseGenerator(new TestGroup()
            {
                TestType = string.Empty
            });

            Assert.IsNotNull(generator);
        }
    }
}
