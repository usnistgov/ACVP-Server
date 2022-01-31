using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Async
{
    [TestFixture, UnitTest]
    public class ResultValidatorAsyncTests
    {
        private Mock<IOptions<OrleansConfig>> _orleansConfig;
        private ResultValidatorAsync<FakeTestGroup, FakeTestCase> _subject;

        [SetUp]
        public void Setup()
        {
            var orleansConfig = new OrleansConfig()
            {
                MaxWorkItemsToQueuePerGenValInstance = 1000
            };
            _orleansConfig = new Mock<IOptions<OrleansConfig>>();
            _orleansConfig.Setup(s => s.Value).Returns(orleansConfig);

            _subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>(_orleansConfig.Object);
        }

        [Test]
        public async Task ShouldReturnValidation()
        {
            var validation = await _subject.ValidateResultsAsync(new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>(), new List<FakeTestGroup>(), false);
            Assert.IsNotNull(validation);
        }

        [Test]
        [TestCase(3)]
        [TestCase(16)]
        public async Task ShouldReturnOnResultValidationPerSuppliedValidator(int count)
        {
            var validators = new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>();
            for (var idx = 0; idx < count; idx++)
            {
                validators.Add(new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) { TestCaseId = idx + 1 });
            }

            var validation = await _subject.ValidateResultsAsync(validators, new List<FakeTestGroup>(), false);

            Assert.That(validation != null);
            Assert.AreEqual(count, validation.Validations.Count);
        }

        [Test]
        public async Task ShouldMarkMissingIfNoMatchingResultPresent()
        {
            var validation =
                await _subject.ValidateResultsAsync(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 2}}}
                    }, false);

            Assert.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assert.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Missing, firstResultValidation.Result);
        }

        [Test]
        public async Task ShouldMarkPassedForValidResult()
        {
            var validation =
                await _subject.ValidateResultsAsync(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 1}}}
                    }, false);

            Assert.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assert.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Passed, firstResultValidation.Result);
        }

        [Test]
        public async Task ShouldMarkFailedForInvalidResult()
        {
            var validation =
                await _subject.ValidateResultsAsync(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Failed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 1}}}
                    }, false);

            Assert.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assert.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Failed, firstResultValidation.Result);
        }

        [Test]
        public async Task ShouldMarkAllResultsProperly()
        {
            var validation =
                await _subject.ValidateResultsAsync(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Failed) {TestCaseId = 1},
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 2},
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 3}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup
                        {
                            TestGroupId = 2,
                            Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 1}, new FakeTestCase {TestCaseId = 2}}
                        }
                    }, false);

            Assert.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 1);
            Assert.AreEqual(Disposition.Failed, firstResultValidation.Result);

            var secondResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 2);
            Assert.AreEqual(Disposition.Passed, secondResultValidation.Result);

            var thirdResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 3);
            Assert.AreEqual(Disposition.Missing, thirdResultValidation.Result);
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
            var validation = new Mock<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>();
            validation
                .Setup(s => s.ValidateAsync(It.IsAny<FakeTestCase>(), It.IsAny<bool>()))
                .Returns(async () =>
                {
                    await Task.Yield();
                    return new TestCaseValidation();
                });

            _subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>(_orleansConfig.Object);

            var validators = new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>();
            var testGroup = new FakeTestGroup()
            {
                Tests = new List<FakeTestCase>()
            };
            for (var i = 0; i < numberToQueue; i++)
            {
                validators.Add(new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed)
                {
                    TestCaseId = i
                });
                testGroup.Tests.Add(new FakeTestCase()
                {
                    TestCaseId = i
                });
            }

            var result = await _subject.ValidateResultsAsync(validators, new List<FakeTestGroup>() { testGroup }, false);

            Assert.AreEqual(numberToQueue, result.Validations.Count());
        }
    }
}
