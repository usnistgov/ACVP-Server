using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {

        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _mockTestCaseGeneratorFactory;
        private Mock<ITestCaseGenerator<TestGroup, TestCase>> _mockTestCaseGenerator;
        private TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _mockTestCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _mockTestCaseGenerator = new Mock<ITestCaseGenerator<TestGroup, TestCase>>();

            _mockTestCaseGenerator
                .Setup(s => s.Generate(It.IsAny<TestGroup>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse(new TestCase()));
            _mockTestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_mockTestCaseGenerator.Object);

            _subject = new TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>();
        }

        [Test]
        [TestCase("gen", typeof(TestCaseValidatorGen<TestCase>))]
        [TestCase("GeN", typeof(TestCaseValidatorGen<TestCase>))]
        [TestCase("ver", typeof(TestCaseValidatorVer<TestCase>))]
        [TestCase("vEr", typeof(TestCaseValidatorVer<TestCase>))]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(string function, Type expectedType)
        {
            TestVectorSet testVectorSet = null;

            GetData(ref testVectorSet, function);

            var results = _subject.GetValidators(testVectorSet);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        private void GetData(ref TestVectorSet testVectorSet, string function)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        Function = function,
                        TestType = string.Empty,
                        KeyLength = 128,
                        MessageLength = 0,
                        MacLength = 64,
                        Tests = new List<ITestCase>()
                        {
                            new TestCase()
                            {
                                FailureTest = false,
                                Key = new BitString(128),
                                Message = new BitString(128),
                                Mac = new BitString(128),
                                TestCaseId = 1
                            }
                        }
                    }
                }
            };
        }
    }
}
