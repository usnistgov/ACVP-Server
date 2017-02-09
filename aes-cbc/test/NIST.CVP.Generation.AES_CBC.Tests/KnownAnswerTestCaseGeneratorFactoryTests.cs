using System;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture]
    public class KnownAnswerTestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not real", typeof(KnownAnswerTestCaseGeneratorNull))]
        [TestCase("gfsbox", typeof(KnownAnswerTestCaseGeneratorGFSBox))]
        [TestCase("GFsBoX", typeof(KnownAnswerTestCaseGeneratorGFSBox))]
        [TestCase("keysbox", typeof(KnownAnswerTestCaseGeneratorKeySBox))]
        [TestCase("keYsbOx", typeof(KnownAnswerTestCaseGeneratorKeySBox))]
        [TestCase("vartxt", typeof(KnownAnswerTestCaseGeneratorVarTxt))]
        [TestCase("vaRTxt", typeof(KnownAnswerTestCaseGeneratorVarTxt))]
        [TestCase("varkey", typeof(KnownAnswerTestCaseGeneratorVarKey))]
        [TestCase("vArkEy", typeof(KnownAnswerTestCaseGeneratorVarKey))]
        public void ShouldReturnCorrectType(string testType, Type type)
        {
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = string.Empty
            };

            KnownAnswerTestCaseGeneratorFactory subject = new KnownAnswerTestCaseGeneratorFactory();
            var result = subject.GetStaticCaseGenerator(testGroup);

            Assert.IsInstanceOf(type, result);
        }
    }
}
