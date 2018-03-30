using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {

        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _mockTestCaseGeneratorFactory;
        private Mock<ITestCaseGenerator<TestGroup, TestCase>> _mockTestCaseGenerator;
        private TestCaseValidatorFactory _subject;

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

            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase(typeof(TestCaseValidator))]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
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

            suppliedResults = new List<TestCase>()
            {
                new TestCase()
                {
                    TestCaseId = 1
                }
            };
        }
    }
}
