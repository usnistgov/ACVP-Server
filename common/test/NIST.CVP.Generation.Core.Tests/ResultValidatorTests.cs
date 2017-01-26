using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class ResultValidatorTests
    {
        [Test]
        public void ShouldReturnValidation()
        {
            var subject = new ResultValidator<ITestCase>();
            var valdiation = subject.ValidateResults(new List<ITestCaseValidator<ITestCase>>(), new List<ITestCase>());
            Assert.IsNotNull(valdiation);
        }

        [Test]
        [TestCase(3)]
        [TestCase(16)]
        public void ShouldReturnOnResultValidationPerSuppliedValidator(int count)
        {
            var validators = new List<ITestCaseValidator<ITestCase>>();
            for (int idx = 0; idx < count; idx++)
            {
                validators.Add(new FakeTestCaseValidator<ITestCase>("passed") {TestCaseId = idx+1});
            }

            var subject = new ResultValidator<ITestCase>();
            var validation = subject.ValidateResults(validators, new List<ITestCase>());
            Assume.That(validation != null);
            Assert.AreEqual(count, validation.Validations.Count);
        }

        [Test]
        public void ShouldMarkMissingIfNoMatchingResultPresent()
        {
            var subject = new ResultValidator<ITestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidator<ITestCase>>
                    {
                        new FakeTestCaseValidator<ITestCase>("passed") {TestCaseId = 1}
                    },
                    new List<ITestCase>() {new FakeTestCase() {TestCaseId = 2}});
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("missing", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkPassedForValidResult()
        {
            var subject = new ResultValidator<ITestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidator<ITestCase>>
                    {
                        new FakeTestCaseValidator<ITestCase>("passed") {TestCaseId = 1}
                    },
                    new List<ITestCase> {new FakeTestCase() {TestCaseId = 1}});
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("passed", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkFailedForInvalidResult()
        {
            var subject = new ResultValidator<ITestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidator<ITestCase>>
                    {
                        new FakeTestCaseValidator<ITestCase>("failed") {TestCaseId = 1}
                    },
                    new List<ITestCase> {new FakeTestCase() {TestCaseId = 1}});
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("failed", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkAllResultsProperly()
        {
            var subject = new ResultValidator<ITestCase>();
            var validation =
                subject.ValidateResults(
                    new List<ITestCaseValidator<ITestCase>>
                    {
                        new FakeTestCaseValidator<ITestCase>("failed") {TestCaseId = 1},
                        new FakeTestCaseValidator<ITestCase>("passed") {TestCaseId = 2},
                        new FakeTestCaseValidator<ITestCase>("passed") {TestCaseId = 3}
                    },
                    new List<ITestCase> {new FakeTestCase() {TestCaseId = 1}, new FakeTestCase {TestCaseId = 2}});
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 1);
            Assert.AreEqual("failed", firstResultValidation.Result);
            var secondResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 2);
            Assert.AreEqual("passed", secondResultValidation.Result);
            var thirdResultValidation = validation.Validations.FirstOrDefault(v => v.TestCaseId == 3);
            Assert.AreEqual("missing", thirdResultValidation.Result);
        }
    }
}
