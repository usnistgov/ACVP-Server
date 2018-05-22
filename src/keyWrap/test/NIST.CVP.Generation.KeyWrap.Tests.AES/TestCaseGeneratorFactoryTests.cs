using System;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt<TestGroup, TestCase>))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt<TestGroup, TestCase>))]
        [TestCase("", typeof(TestCaseGeneratorNull<TestGroup, TestCase>))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Direction = direction
            };

            var subject = new TestCaseGeneratorFactory<TestGroup, TestCase>(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
