using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.TDES_ECB.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "InversePermutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "InversePermutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "Permutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "Permutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "VariableKey", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "VariableKey", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "VariableText", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "VariableText", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "SubstitutionTable", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "SubstitutionTable", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "InversePermutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "InversePermutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "Permutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "Permutation", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "VariableKey", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "VariableKey", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "VariableText", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "VariableText", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "SubstitutionTable", "", typeof(TestCaseGeneratorKat))]
        [TestCase("decrypt", "SubstitutionTable", "", typeof(TestCaseGeneratorKat))]
        [TestCase("encrypt", "MultiBlockMessage", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", "MultiBlockMessage", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", "MultiBlockMessage", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Junk", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "", "MCT", typeof(TestCaseGeneratorMct))]
        public void ShouldReturnProperGenerator(string direction, string internalTestType, string testType, Type expectedType)
        {
            var testGroup = new TestGroup()
            {
                Function = direction,
                InternalTestType = internalTestType,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
        
        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(new TestGroup {Function = "", TestType = ""});
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var oracle = new Mock<IOracle>().Object;
            
            return new TestCaseGeneratorFactory(oracle);
        }
    }
}
