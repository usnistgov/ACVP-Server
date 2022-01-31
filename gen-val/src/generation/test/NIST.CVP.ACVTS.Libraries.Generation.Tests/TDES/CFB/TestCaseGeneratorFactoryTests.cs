using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFB
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "", "InversePermutation", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "InversePermutation", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "Permutation", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "Permutation", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "VariableKey", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "VariableKey", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "VariableText", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "VariableText", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "SubstitutionTable", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "SubstitutionTable", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "InversePermutation", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "InversePermutation", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "Permutation", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "Permutation", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "VariableKey", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "VariableKey", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "VariableText", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "VariableText", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "SubstitutionTable", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "", "SubstitutionTable", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", "", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", "", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("Junk", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MCT", "", typeof(TestCaseGeneratorMct))]
        public void ShouldReturnProperGenerator(string direction, string testType, string internalTestType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = AlgoMode.TDES_CFB1_v1_0,
                Function = direction,
                TestType = testType,
                InternalTestType = internalTestType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(new TestGroup { Function = "", TestType = "" });
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var oracle = new Mock<IOracle>().Object;

            return new TestCaseGeneratorFactory(oracle);
        }
    }
}
