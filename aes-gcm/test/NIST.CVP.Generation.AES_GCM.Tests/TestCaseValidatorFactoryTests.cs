using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
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

            _subject = new TestCaseValidatorFactory(_mockTestCaseGeneratorFactory.Object);
        }

        [Test]
        [TestCase("encrypt", false, typeof(TestCaseValidatorEncrypt))]
        [TestCase("encrypt", true, typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", false, typeof(TestCaseValidatorDecrypt))]
        [TestCase("decrypt", true, typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(string function, bool isDeferred, Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, function, isDeferred);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        [Test]
        [TestCase("encrypt", false, 0)]
        [TestCase("encrypt", true, 1)]
        [TestCase("decrypt", false, 0)]
        [TestCase("decrypt", true, 1)]
        public void ShouldOnlyCallGetCaseGeneratorOnDeferredGroups(string function, bool isDeferred, int timesToCall)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, function, isDeferred);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            _mockTestCaseGenerator.Verify(v => v.Generate(It.IsAny<TestGroup>(), It.IsAny<TestCase>()), Times.Exactly(timesToCall), nameof(_mockTestCaseGenerator.Object.Generate));
        }

        [Test]
        public void ShouldThrowExceptionIfGeneratorFails()
        {
            _mockTestCaseGenerator
                .Setup(s => s.Generate(It.IsAny<TestGroup>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse("Error"));

            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, "encrypt", true);

            Assert.Throws(typeof(Exception), () => _subject.GetValidators(testVectorSet, suppliedResults));
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults, string function, bool isDeferred)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        Function = function,
                        AADLength = 128,
                        TestType = string.Empty,
                        IVGeneration = string.Empty,
                        IVGenerationMode = string.Empty,
                        IVLength = 128,
                        KeyLength = 128,
                        PTLength = 128,
                        TagLength = 128,
                        Tests = new List<ITestCase>()
                        {
                            new TestCase()
                            {
                                AAD = new BitString(128),
                                CipherText = new BitString(128),
                                Deferred = isDeferred,
                                FailureTest = false,
                                Key = new BitString(128),
                                PlainText = new BitString(128),
                                Tag = new BitString(128),
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
                    IV = new BitString(128),
                    TestCaseId = 1
                }
            };
        }
    }
}
