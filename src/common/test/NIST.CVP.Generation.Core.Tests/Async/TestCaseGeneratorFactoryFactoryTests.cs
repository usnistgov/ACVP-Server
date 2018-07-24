using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Async
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryFactoryTests
    {
        private Mock<ITestCaseGeneratorFactoryAsync<FakeTestGroup, FakeTestCase>> _testCaseGeneratorFactory;
        private Mock<ITestCaseGeneratorAsync<FakeTestGroup, FakeTestCase>> _testCaseGenerator;
        private Core.Async.TestCaseGeneratorFactoryFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> _subject;
        private FakeTestVectorSet _testVectorSet;

        [SetUp]
        public void Setup()
        {
            _testCaseGenerator = new Mock<ITestCaseGeneratorAsync<FakeTestGroup, FakeTestCase>>();
            _testCaseGenerator
                .SetupGet(s => s.NumberOfTestCasesToGenerate)
                .Returns(1);
            _testCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<FakeTestGroup>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<FakeTestGroup, FakeTestCase>(new FakeTestCase())));

            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactoryAsync<FakeTestGroup, FakeTestCase>>();
            _testCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<FakeTestGroup>()))
                .Returns(_testCaseGenerator.Object);

            _subject =
                new Core.Async.TestCaseGeneratorFactoryFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>(
                    _testCaseGeneratorFactory.Object);
            _testVectorSet = new FakeTestVectorSet()
            {
                Algorithm = "",
                TestGroups = new List<FakeTestGroup>()
                {
                    new FakeTestGroup()
                }
            };
        }

        [Test]
        public void ShouldReturnErrorMessageWithinGenerateResponseWhenFails()
        {
            _testCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<FakeTestGroup>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<FakeTestGroup, FakeTestCase>("fail")));

            var result = _subject.BuildTestCases(_testVectorSet);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage), nameof(result.ErrorMessage));
        }

        [Test]
        public void ShouldReturnGenerateResponseWhenGenerateSuccess()
        {
            var results = _subject.BuildTestCases(_testVectorSet);

            Assert.IsTrue(results.Success);
            _testCaseGeneratorFactory
                .Verify(v => v.GetCaseGenerator(
                        It.IsAny<FakeTestGroup>()),
                    Times.Exactly(1),
                    nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator)
                );
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldCallGetGeneratorForEachGroup(int numberOfGroups)
        {
            _testVectorSet = new FakeTestVectorSet()
            {
                TestGroups = new List<FakeTestGroup>()
            };

            for (int i = 0; i < numberOfGroups; i++)
            {
                _testVectorSet.TestGroups.Add(new FakeTestGroup());
            }

            var results = _subject.BuildTestCases(_testVectorSet);

            Assume.That(results.Success);
            _testCaseGeneratorFactory
                .Verify(
                    v => v.GetCaseGenerator(It.IsAny<FakeTestGroup>()), 
                    Times.Exactly(numberOfGroups), 
                    nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator)
                );
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldCallGenerateSpecifiedNumberOfTimes(int numberOfTestCasesToGenerate)
        {
            _testCaseGenerator
                .SetupGet(s => s.NumberOfTestCasesToGenerate)
                .Returns(numberOfTestCasesToGenerate);

            var results = _subject.BuildTestCases(_testVectorSet);

            Assume.That(results.Success);
            _testCaseGenerator
                .Verify(
                    v => v.GenerateAsync(
                        It.IsAny<FakeTestGroup>(), 
                        It.IsAny<bool>()
                    ), 
                    Times.Exactly(numberOfTestCasesToGenerate),
                    nameof(_testCaseGenerator.Object.GenerateAsync)
                );
        }
    }
}