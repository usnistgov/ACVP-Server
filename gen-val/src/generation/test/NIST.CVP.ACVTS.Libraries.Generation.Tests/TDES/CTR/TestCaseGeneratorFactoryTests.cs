﻿using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;
        private Mock<IOracle> _oracle;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();

            _subject = new TestCaseGeneratorFactory(_oracle.Object);
        }

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
        [TestCase("encrypt", "", "singleblock", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("Encrypt", "", "sInGlEbLoCk", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("DEcrypt", "", "SINGLEBLOCK", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("Decrypt", "", "SingleBlock", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("encrypt", "", "PartialBlock", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("ENCRYPT", "", "PARTIALBLOCK", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("deCRYPT", "", "partialBLOCK", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("Decrypt", "", "PaRtIaLbLoCk", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("EncRypt", "ctr", "", typeof(TestCaseGeneratorCounter))]
        [TestCase("ENCRYPT", "CTR", "", typeof(TestCaseGeneratorCounter))]
        [TestCase("decrypt", "Ctr", "", typeof(TestCaseGeneratorCounter))]
        [TestCase("Decrypt", "CTr", "", typeof(TestCaseGeneratorCounter))]
        [TestCase("Junk", "Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string testType, string internalTestType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Direction = direction,
                TestType = testType,
                InternalTestType = internalTestType
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var testGroup = new TestGroup
            {
                Direction = string.Empty,
                TestType = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(_oracle.Object);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator, Is.Not.Null);
        }
    }
}
