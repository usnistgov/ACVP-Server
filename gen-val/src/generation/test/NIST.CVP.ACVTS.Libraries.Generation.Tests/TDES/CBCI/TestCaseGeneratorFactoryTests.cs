using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBCI.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBCI
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
                Function = direction,
                TestType = testType,
                InternalTestType = internalTestType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(new TestGroup { Function = "", TestType = "" });
            Assert.That(generator, Is.Not.Null);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var oracle = new Mock<IOracle>().Object;

            return new TestCaseGeneratorFactory(oracle);
        }
    }
}
