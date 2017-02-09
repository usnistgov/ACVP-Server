using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
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
                TestType = testType
            };

            KnownAnswerTestCaseGeneratorFactory subject = new KnownAnswerTestCaseGeneratorFactory();
            var result = subject.GetStaticCaseGenerator(testGroup);

            Assert.IsInstanceOf(type, result);
        }
    }
}
