using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.AES_CFB8.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseGeneratorFactory(null);
        }

        [Test]
        [TestCase("not relevant", 128, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 128, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 128, "MmT", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 128, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 128, "mMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 128, "MMt", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 128, "mCt", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 128, "MCT", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 128, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 128, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 128, "McT", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 128, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 192, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 192, "MmT", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 192, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 192, "mMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 192, "MMt", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 192, "mCt", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 192, "MCT", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 192, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 192, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 192, "McT", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 192, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 256, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 256, "MmT", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 256, "MMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 256, "mMT", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 256, "MMt", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 256, "mCt", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 256, "MCT", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 256, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 256, "mct", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 256, "McT", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 256, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, int keySize, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                Function = direction,
                KeyLength = keySize,
                TestType = testType
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var generator = _subject.GetCaseGenerator(new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                Function = string.Empty,
                TestType = string.Empty
            });
            Assert.IsNotNull(generator);
        }
    }
}
