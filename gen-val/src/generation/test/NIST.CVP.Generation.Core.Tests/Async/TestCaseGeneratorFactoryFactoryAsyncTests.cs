using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoC;
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Async
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryFactoryAsyncTests
    {
        private Mock<ITestCaseGeneratorFactoryAsync<FakeTestGroup, FakeTestCase>> _testCaseGeneratorFactory;
        private Mock<ITestCaseGeneratorAsync<FakeTestGroup, FakeTestCase>> _testCaseGenerator;
        private Mock<IOptions<OrleansConfig>> _orleansConfig;
        private TestCaseGeneratorFactoryFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> _subject;
        private FakeTestVectorSet _testVectorSet;

        [SetUp]
        public void Setup()
        {
            _testCaseGenerator = new Mock<ITestCaseGeneratorAsync<FakeTestGroup, FakeTestCase>>();
            _testCaseGenerator
                .SetupGet(s => s.NumberOfTestCasesToGenerate)
                .Returns(1);
            _testCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<FakeTestGroup>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<FakeTestGroup, FakeTestCase>(new FakeTestCase())));

            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactoryAsync<FakeTestGroup, FakeTestCase>>();
            _testCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<FakeTestGroup>()))
                .Returns(_testCaseGenerator.Object);

            var orleansConfig = new OrleansConfig()
            {
                MaxWorkItemsToQueuePerGenValInstance = 1000
            };
            _orleansConfig = new Mock<IOptions<OrleansConfig>>();
            _orleansConfig.Setup(s => s.Value).Returns(orleansConfig);
            
            _subject =
                new TestCaseGeneratorFactoryFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>(
                    _testCaseGeneratorFactory.Object, _orleansConfig.Object);
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
        public async Task ShouldReturnErrorMessageWithinGenerateResponseWhenFails()
        {
            _testCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<FakeTestGroup>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new TestCaseGenerateResponse<FakeTestGroup, FakeTestCase>("fail")));

            var result = await _subject.BuildTestCasesAsync(_testVectorSet);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage), nameof(result.ErrorMessage));
        }

        [Test]
        public async Task ShouldReturnGenerateResponseWhenGenerateSuccess()
        {
            var results = await _subject.BuildTestCasesAsync(_testVectorSet);

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
        public async Task ShouldCallGetGeneratorForEachGroup(int numberOfGroups)
        {
            _testVectorSet = new FakeTestVectorSet()
            {
                TestGroups = new List<FakeTestGroup>()
            };

            for (int i = 0; i < numberOfGroups; i++)
            {
                _testVectorSet.TestGroups.Add(new FakeTestGroup());
            }

            var results = await _subject.BuildTestCasesAsync(_testVectorSet);

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
        public async Task ShouldCallGenerateSpecifiedNumberOfTimes(int numberOfTestCasesToGenerate)
        {
            _testCaseGenerator
                .SetupGet(s => s.NumberOfTestCasesToGenerate)
                .Returns(numberOfTestCasesToGenerate);

            var results = await _subject.BuildTestCasesAsync(_testVectorSet);

            Assume.That(results.Success);
            _testCaseGenerator
                .Verify(
                    v => v.GenerateAsync(
                        It.IsAny<FakeTestGroup>(), 
                        It.IsAny<bool>(),
                        It.IsAny<int>()
                    ), 
                    Times.Exactly(numberOfTestCasesToGenerate),
                    nameof(_testCaseGenerator.Object.GenerateAsync)
                );
        }

        [Test]
        [TestCase(5, 10)] // less than max
        [TestCase(15, 5)] // 3x max (will need to wait a few times)
        [TestCase(500, 100)] // 5x max, larger max, will need to wait a few times
        [TestCase(499, 102)] // maxQueue and number to queue aren't evenly divisible
        public async Task ShouldEnqueueWorkUpToMaxAmount(int numberToQueue, int maxQueue)
        {
            var orleansConfig = new OrleansConfig()
            {
                MaxWorkItemsToQueuePerGenValInstance = maxQueue
            };
            _orleansConfig.Setup(s => s.Value).Returns(orleansConfig);
            _testCaseGenerator.SetupGet(s => s.NumberOfTestCasesToGenerate).Returns(numberToQueue);
            _testCaseGenerator
                .Setup(s => s.GenerateAsync(It.IsAny<FakeTestGroup>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(async () =>
                {
                    await Task.Delay(10);
                    return new TestCaseGenerateResponse<FakeTestGroup, FakeTestCase>(new FakeTestCase());
                });
            _subject =
                new TestCaseGeneratorFactoryFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>(
                    _testCaseGeneratorFactory.Object, _orleansConfig.Object);

            var testGroup = new FakeTestGroup()
            {
                Tests = new List<FakeTestCase>() 
            };
            var vectorSet = new FakeTestVectorSet()
            {
                TestGroups = new List<FakeTestGroup>()
                {
                    testGroup
                }
            };
            
            await _subject.BuildTestCasesAsync(vectorSet);

            Assert.AreEqual(numberToQueue, vectorSet.TestGroups.SelectMany(s => s.Tests).Count());
        }
    }
}