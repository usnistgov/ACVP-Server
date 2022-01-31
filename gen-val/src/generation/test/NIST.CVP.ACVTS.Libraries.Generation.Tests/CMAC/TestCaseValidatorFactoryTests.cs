using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {

        private Mock<ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>> _mockTestCaseGeneratorFactory;
        private Mock<ITestCaseGeneratorAsync<TestGroup, TestCase>> _mockTestCaseGenerator;
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _mockTestCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>>();
            _mockTestCaseGenerator = new Mock<ITestCaseGeneratorAsync<TestGroup, TestCase>>();

            _mockTestCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<TestGroup>(), true, It.IsAny<int>()))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase())));
            _mockTestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_mockTestCaseGenerator.Object);

            _subject = new TestCaseValidatorFactory();
        }

        [Test]
        [TestCase("gen", typeof(TestCaseValidatorGen))]
        [TestCase("GeN", typeof(TestCaseValidatorGen))]
        [TestCase("ver", typeof(TestCaseValidatorVer))]
        [TestCase("vEr", typeof(TestCaseValidatorVer))]
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
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        Function = function,
                        TestType = string.Empty,
                        KeyLength = 128,
                        MessageLength = 0,
                        MacLength = 64,
                        Tests = new List<TestCase>()
                        {
                            new TestCase()
                            {
                                TestPassed = true,
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
