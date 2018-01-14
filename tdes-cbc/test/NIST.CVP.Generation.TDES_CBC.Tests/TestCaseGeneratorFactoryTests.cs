using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CBC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
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
            var algo = new Mock<ITDES_CBC>().Object;
            var algoMct = new Mock<ITDES_CBC_MCT>().Object;
            return new TestCaseGeneratorFactory(randy, algo, algoMct);
        }
    }
}