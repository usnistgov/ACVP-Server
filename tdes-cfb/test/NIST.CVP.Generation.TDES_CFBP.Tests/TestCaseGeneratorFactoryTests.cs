using System;
using Moq;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CFBP;
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
        [TestCase("encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        [TestCase("Encrypt", "", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "MCT", typeof(TestCaseGeneratorMonteCarloEncrypt))]
        [TestCase("Decrypt", "MultiBlockMessage", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("Decrypt", "MCT", typeof(TestCaseGeneratorMonteCarloDecrypt))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
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
            var randy = new Mock<IRandom800_90>().Object;
            var algo = new Mock<ICFBPMode>();
            algo.SetupGet(s => s.Algo).Returns(AlgoMode.TDES_CFBP1);
            var algoMct = new Mock<ICFBPModeMCT>();
            algoMct.SetupGet(s => s.Algo).Returns(AlgoMode.TDES_CFBP1);
            return new TestCaseGeneratorFactory(randy, algo.Object, algoMct.Object);
        }
    }
}