using System;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Direction = direction
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
