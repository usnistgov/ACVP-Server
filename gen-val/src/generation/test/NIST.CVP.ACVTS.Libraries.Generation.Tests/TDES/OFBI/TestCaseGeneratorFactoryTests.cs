using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFBI.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.OFBI
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
        [TestCase("encrypt", "aft", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", "AFT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", "AfT", "", typeof(TestCaseGeneratorMmt))]
        [TestCase("Junk", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MCT", "", typeof(TestCaseGeneratorMonteCarlo))]
        public void ShouldReturnProperGenerator(string direction, string testType, string katType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = direction,
                TestType = testType,
                InternalTestType = katType
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
            var generator = subject.GetCaseGenerator(new TestGroup { Function = "", TestType = "", InternalTestType = "" });
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var oracle = new Mock<IOracle>().Object;
            return new TestCaseGeneratorFactory(oracle);
        }
    }
}
