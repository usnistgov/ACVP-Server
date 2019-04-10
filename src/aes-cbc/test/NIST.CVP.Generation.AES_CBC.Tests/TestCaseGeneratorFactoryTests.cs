using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using NIST.CVP.Generation.AES_CBC.v1_0;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not relevant", 128, "AFT", "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "AFT", "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "AFT", "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "AFT", "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 128, "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 128, "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 128, "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 128, "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 128, "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 128, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 128, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 128, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 128, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 128, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 128, "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "AFT", "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "AFT", "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "AFT", "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "AFT", "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 192, "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 192, "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 192, "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 192, "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 192, "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 192, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 192, "MCT", "", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 192, "mct", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 192, "mct", "", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 192, "McT", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 192, "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "Aft", "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "Aft", "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "Aft", "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "Aft", "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 256, "aFt", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", 256, "aFt", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", 256, "aFt", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Decrypt", 256, "aFt", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("decrypt", 256, "aFt", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("encrypt", 256, "mCt", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 256, "MCT", "", typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 256, "mct", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 256, "mct", "", typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 256, "McT", "", typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 256, "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, int keySize, string testType, string katType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                KeyLength = keySize,
                TestType = testType,
                InternalTestType = katType
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(new TestGroup()
            {
                Function = string.Empty,
                TestType = string.Empty
            });
            Assert.IsNotNull(generator);
        }
    }
}
