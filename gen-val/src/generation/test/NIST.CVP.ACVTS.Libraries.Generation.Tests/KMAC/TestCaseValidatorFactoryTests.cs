using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KMAC
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
        [TestCase(typeof(TestCaseValidatorAft), false)]
        [TestCase(typeof(TestCaseValidatorMvt), true)]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(Type expectedType, bool mvt)
        {
            TestVectorSet testVectorSet = null;

            GetData(ref testVectorSet, mvt);

            var results = _subject.GetValidators(testVectorSet);

            Assert.That(results.Count() == 1, Is.True, "Expected 1 validator");
            Assert.That(results.First(), Is.InstanceOf(expectedType));
        }

        private void GetData(ref TestVectorSet testVectorSet, bool mvt)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = mvt ? "mvt" : string.Empty,
                        KeyLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 256, 512)),
                        MessageLength = 0,
                        Tests = new List<TestCase>()
                        {
                            new TestCase()
                            {
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
