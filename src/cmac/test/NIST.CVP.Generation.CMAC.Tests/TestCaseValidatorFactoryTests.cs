using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {

        private Mock<ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>> _mockTestCaseGeneratorFactory;
        private Mock<ITestCaseGeneratorAsync<TestGroup, TestCase>> _mockTestCaseGenerator;
        private TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _mockTestCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>>();
            _mockTestCaseGenerator = new Mock<ITestCaseGeneratorAsync<TestGroup, TestCase>>();

            _mockTestCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<TestGroup>(), true))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase())));
            _mockTestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_mockTestCaseGenerator.Object);

            _subject = new TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>();
        }

        [Test]
        [TestCase("gen", typeof(TestCaseValidatorGen<TestGroup, TestCase>))]
        [TestCase("GeN", typeof(TestCaseValidatorGen<TestGroup, TestCase>))]
        [TestCase("ver", typeof(TestCaseValidatorVer<TestGroup, TestCase>))]
        [TestCase("vEr", typeof(TestCaseValidatorVer<TestGroup, TestCase>))]
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
