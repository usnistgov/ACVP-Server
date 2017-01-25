using System;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class StaticTestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not real", typeof(StaticTestCaseGeneratorNull))]
        [TestCase("gfsbox", typeof(StaticTestCaseGeneratorGFSBox))]
        [TestCase("GFsBoX", typeof(StaticTestCaseGeneratorGFSBox))]
        [TestCase("keysbox", typeof(StaticTestCaseGeneratorKeySBox))]
        [TestCase("keYsbOx", typeof(StaticTestCaseGeneratorKeySBox))]
        [TestCase("vartxt", typeof(StaticTestCaseGeneratorVarTxt))]
        [TestCase("vaRTxt", typeof(StaticTestCaseGeneratorVarTxt))]
        [TestCase("varkey", typeof(StaticTestCaseGeneratorVarKey))]
        [TestCase("vArkEy", typeof(StaticTestCaseGeneratorVarKey))]
        public void ShouldReturnCorrectType(string testType, Type type)
        {
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = string.Empty
            };

            StaticTestCaseGeneratorFactory subject = new StaticTestCaseGeneratorFactory();
            var result = subject.GetStaticCaseGenerator(testGroup);

            Assert.IsInstanceOf(type, result);
        }
    }
}
