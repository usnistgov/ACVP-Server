using System.Collections.Generic;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryFactoryTests
    {
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _testCaseGeneratorFactory;
        private Mock<ITestCaseGenerator<TestGroup, TestCase>> _testCaseGenerator;
        private TestCaseGeneratorFactoryFactory _subject;
        private TestVectorSet _testVectorSet;

        [SetUp]
        public void Setup()
        {
            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _testCaseGenerator = new Mock<ITestCaseGenerator<TestGroup, TestCase>>();
            _subject = new TestCaseGeneratorFactoryFactory(_testCaseGeneratorFactory.Object);

            _testVectorSet = new TestVectorSet
            {
                Algorithm = "",
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        TestType = "kat",
                        Modulo = 2048
                    },
                    new TestGroup
                    {
                        TestType = "aft",
                        Modulo = 3072
                    },
                    new TestGroup
                    {
                        TestType = "gdt",
                        Modulo = 4096
                    }
                }
            };

            _testCaseGeneratorFactory.Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new FakeTestCaseGenerator<TestGroup, TestCase>());
        }

        [Test]
        public void ShouldReturnGenerateResponseWhenGenerateSuccess()
        {
            var results = _subject.BuildTestCases(_testVectorSet);

            Assert.IsTrue(results.Success);
            _testCaseGeneratorFactory
                .Verify(v => v.GetCaseGenerator(
                        It.IsAny<TestGroup>()),
                    Times.AtLeastOnce(),
                    nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator)
                );
        }

        [Test]
        public void ShouldRetryAFTAfterFailedTest()
        {
            _testCaseGenerator
                .Setup(s => s.Generate(It.IsAny<TestGroup>(), It.IsAny<bool>()))
                .Returns(new Queue<TestCaseGenerateResponse>(
                    new[] 
                    {
                        new TestCaseGenerateResponse("Repeat"),
                        new TestCaseGenerateResponse(new TestCase()),
                        new TestCaseGenerateResponse("Repeat"),
                        new TestCaseGenerateResponse(new TestCase())
                    }).Dequeue);

            _testCaseGenerator
                .Setup(s => s.NumberOfTestCasesToGenerate)
                .Returns(1);

            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _testCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_testCaseGenerator.Object);

            _subject = new TestCaseGeneratorFactoryFactory(_testCaseGeneratorFactory.Object);

            var results = _subject.BuildTestCases(_testVectorSet);

            Assert.IsTrue(results.Success);
            _testCaseGenerator
                .Verify(v => v.Generate(It.IsAny<TestGroup>(), It.IsAny<bool>()), Times.AtLeast(2));
        }
    }
}
