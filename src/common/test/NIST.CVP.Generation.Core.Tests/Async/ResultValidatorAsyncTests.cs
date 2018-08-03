using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Async
{
    [TestFixture, UnitTest]
    public class ResultValidatorAsyncTests
    {
        [Test]
        public void ShouldReturnValidation()
        {
            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var valdiation = subject.ValidateResults(new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>(), new List<FakeTestGroup>(), false);
            Assert.IsNotNull(valdiation);
        }

        [Test]
        [TestCase(3)]
        [TestCase(16)]
        public void ShouldReturnOnResultValidationPerSuppliedValidator(int count)
        {
            var validators = new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>();
            for (var idx = 0; idx < count; idx++)
            {
                validators.Add(new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = idx+1});
            }

            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var validation = subject.ValidateResults(validators, new List<FakeTestGroup>(), false);

            Assume.That(validation != null);
            Assert.AreEqual(count, validation.Validations.Count);
        }

        [Test]
        public void ShouldMarkMissingIfNoMatchingResultPresent()
        {
            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 2}}}
                    }, false);

            Assume.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assume.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Missing, firstResultValidation.Result);
        }

        [Test]
        public void ShouldMarkPassedForValidResult()
        {
            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Passed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 1}}}
                    }, false);

            Assume.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assume.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Passed, firstResultValidation.Result);
        }

        [Test]
        public void ShouldMarkFailedForInvalidResult()
        {
            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>
                    {
                        new FakeTestCaseValidator<FakeTestGroup, FakeTestCase>(Disposition.Failed) {TestCaseId = 1}
                    },
                    new List<FakeTestGroup>
                    {
                        new FakeTestGroup {TestGroupId = 2, Tests = new List<FakeTestCase>{new FakeTestCase {TestCaseId = 1}}}
                    }, false);

            Assume.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault();

            Assume.That(firstResultValidation != null);
            Assert.AreEqual(Disposition.Failed, firstResultValidation.Result);
        }

        [Test]
        public void ShouldMarkAllResultsProperly()
        {
            var subject = new ResultValidatorAsync<FakeTestGroup, FakeTestCase>();
            var validation =
                subject.ValidateResults(
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

            Assume.That(validation != null);

            var firstResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 1);
            Assert.AreEqual(Disposition.Failed, firstResultValidation.Result);
            
            var secondResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 2);
            Assert.AreEqual(Disposition.Passed, secondResultValidation.Result);
            
            var thirdResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 3);
            Assert.AreEqual(Disposition.Missing, thirdResultValidation.Result);
        }
    }
}
