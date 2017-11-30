using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.AES_CTR;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class KnownAnswerTestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not real", typeof(KnownAnswerTestCaseGeneratorNull))]
        [TestCase("gfsbox", typeof(KnownAnswerTestCaseGeneratorGfSBox))]
        [TestCase("GFsBoX", typeof(KnownAnswerTestCaseGeneratorGfSBox))]
        [TestCase("keysbox", typeof(KnownAnswerTestCaseGeneratorKeySBox))]
        [TestCase("keYsbOx", typeof(KnownAnswerTestCaseGeneratorKeySBox))]
        [TestCase("vartxt", typeof(KnownAnswerTestCaseGeneratorVarTxt))]
        [TestCase("vaRTxt", typeof(KnownAnswerTestCaseGeneratorVarTxt))]
        [TestCase("varkey", typeof(KnownAnswerTestCaseGeneratorVarKey))]
        [TestCase("vArkEy", typeof(KnownAnswerTestCaseGeneratorVarKey))]
        public void ShouldReturnCorrectType(string testType, Type type)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Direction = string.Empty
            };

            var subject = new KnownAnswerTestCaseGeneratorFactory();
            var result = subject.GetStaticCaseGenerator(testGroup);

            Assert.IsInstanceOf(type, result);
        }
    }
}
