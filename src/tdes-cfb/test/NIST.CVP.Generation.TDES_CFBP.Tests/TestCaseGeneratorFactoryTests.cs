using System;
using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "InversePermutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "InversePermutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "Permutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "Permutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "VariableKey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "VariableKey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "VariableText", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "VariableText", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "SubstitutionTable", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "SubstitutionTable", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "InversePermutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "InversePermutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "Permutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "Permutation", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "VariableKey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "VariableKey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "VariableText", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "VariableText", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "SubstitutionTable", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("decrypt", "SubstitutionTable", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("Encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("ENcrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMmt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MCT", typeof(TestCaseGeneratorMct))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = AlgoMode.TDES_CFBP1,
                Function = direction,
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