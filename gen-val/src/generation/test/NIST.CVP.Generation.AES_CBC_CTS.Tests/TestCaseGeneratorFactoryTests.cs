using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.AES_CBC_CTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not relevant", 128, "AFT", "gfsbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 128, "AFT", "keysbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 128, "AFT", "vartxt", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 128, "AFT", "varkey", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("encrypt", 128, "AFT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Encrypt", 128, "AFT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("ENcrypt", 128, "AFT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Decrypt", 128, "AFT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("decrypt", 128, "AFT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        //[TestCase("encrypt", 128, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Encrypt", 128, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("ENcrypt", 128, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Decrypt", 128, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("decrypt", 128, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 128, "", "", false, typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", "", false, typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "AFT", "gfsbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 192, "AFT", "keysbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 192, "AFT", "vartxt", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 192, "AFT", "varkey", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("encrypt", 192, "AfT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Encrypt", 192, "AfT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("ENcrypt", 192, "AfT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Decrypt", 192, "AfT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("decrypt", 192, "AfT", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        //[TestCase("encrypt", 192, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Encrypt", 192, "MCT", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("ENcrypt", 192, "mct", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Decrypt", 192, "mct", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("decrypt", 192, "McT", "", false, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 192, "", "", false, typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", "", false, typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "Aft", "gfsbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 256, "Aft", "keysbox", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 256, "Aft", "vartxt", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("not relevant", 256, "Aft", "varkey", false, typeof(TestCaseGeneratorKnownAnswerSingleBlock))]
        [TestCase("encrypt", 256, "aFt", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Encrypt", 256, "aFt", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("ENcrypt", 256, "aFt", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("Decrypt", 256, "aFt", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        [TestCase("decrypt", 256, "aFt", "", false, typeof(TestCaseGeneratorMmtFullBlock))]
        //[TestCase("encrypt", 256, "mCt", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Encrypt", 256, "MCT", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("ENcrypt", 256, "mct", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("Decrypt", 256, "mct", "", false, typeof(TestCaseGeneratorMct))]
        //[TestCase("decrypt", 256, "McT", "", false, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 256, "", "", false, typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", "", false, typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 128, "AFT", "gfsbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 128, "AFT", "keysbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 128, "AFT", "vartxt", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 128, "AFT", "varkey", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("encrypt", 128, "AFT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Encrypt", 128, "AFT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("ENcrypt", 128, "AFT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Decrypt", 128, "AFT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("decrypt", 128, "AFT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("encrypt", 128, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 128, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 128, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 128, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 128, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 128, "", "", true, typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", "", true, typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "AFT", "gfsbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 192, "AFT", "keysbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 192, "AFT", "vartxt", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 192, "AFT", "varkey", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("encrypt", 192, "AfT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Encrypt", 192, "AfT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("ENcrypt", 192, "AfT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Decrypt", 192, "AfT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("decrypt", 192, "AfT", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("encrypt", 192, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 192, "MCT", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 192, "mct", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 192, "mct", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 192, "McT", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 192, "", "", true, typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", "", true, typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "Aft", "gfsbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 256, "Aft", "keysbox", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 256, "Aft", "vartxt", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("not relevant", 256, "Aft", "varkey", true, typeof(TestCaseGeneratorKnownAnswerPartialBlock))]
        [TestCase("encrypt", 256, "aFt", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Encrypt", 256, "aFt", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("ENcrypt", 256, "aFt", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("Decrypt", 256, "aFt", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("decrypt", 256, "aFt", "", true, typeof(TestCaseGeneratorMmtPartialBlock))]
        [TestCase("encrypt", 256, "mCt", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Encrypt", 256, "MCT", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("ENcrypt", 256, "mct", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Decrypt", 256, "mct", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("decrypt", 256, "McT", "", true, typeof(TestCaseGeneratorMct))]
        [TestCase("Junk", 256, "", "", true, typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", "", true, typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, int keySize, string testType, string katType, bool isPartialBlockGroup, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                KeyLength = keySize,
                TestType = testType,
                InternalTestType = katType,
                IsPartialBlockGroup = isPartialBlockGroup
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
