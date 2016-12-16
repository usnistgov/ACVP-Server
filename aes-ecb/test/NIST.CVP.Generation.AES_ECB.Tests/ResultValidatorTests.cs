using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_ECB.Tests.Fakes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class ResultValidatorTests
    {
        [Test]
        public void ShouldReturnValidation()
        {
            var subject = new ResultValidator();
            var valdiation = subject.ValidateResults(new List<ITestCaseValidator>(), new List<TestCase>());
            Assert.IsNotNull(valdiation);
        }

        [Test]
        [TestCase(3)]
        [TestCase(16)]
        public void ShouldReturnOnResultValidationPerSuppliedValidator(int count)
        {
            var validators = new List<ITestCaseValidator>();
            for (int idx = 0; idx < count; idx++)
            {
                validators.Add(new TestCaseValidatorFake("passed") {TestCaseId = idx+1});
            }

            var subject = new ResultValidator();
            var validation = subject.ValidateResults(validators, new List<TestCase>());
            Assume.That(validation != null);
            Assert.AreEqual(count, validation.Validations.Count);
        }

        [Test]
        public void ShouldMarkMissingIfNoMatchingResultPresent()
        {
            var subject = new ResultValidator();
            var validation = subject.ValidateResults(new List<ITestCaseValidator> {new TestCaseValidatorEncrypt(new TestCase {TestCaseId = 1})}, new List<TestCase>());
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("missing", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkPassedForValidResult()
        {
            var subject = new ResultValidator();
            var validation = subject.ValidateResults(new List<ITestCaseValidator> { new TestCaseValidatorFake("passed"){ TestCaseId = 1 }}, new List<TestCase> { new TestCase {TestCaseId = 1} });
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("passed", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkFailedForInvalidResult()
        {
            var subject = new ResultValidator();
            var validation = subject.ValidateResults(new List<ITestCaseValidator> { new TestCaseValidatorFake("failed") { TestCaseId = 1 } }, new List<TestCase> { new TestCase { TestCaseId = 1 } });
            Assume.That(validation != null);
            var firstResultValidation = validation.Validations.FirstOrDefault();
            Assume.That(firstResultValidation != null);
            Assert.AreEqual("failed", firstResultValidation.Result);

        }

        [Test]
        public void ShouldMarkAllResultsProperly()
        {
            var subject = new ResultValidator();
            var validation = subject.ValidateResults(new List<ITestCaseValidator> { new TestCaseValidatorFake("failed") { TestCaseId = 1 }, new TestCaseValidatorFake("passed") { TestCaseId = 2 }, new TestCaseValidatorFake("passed") { TestCaseId = 3 } }, new List<TestCase> { new TestCase { TestCaseId = 1 }, new TestCase { TestCaseId = 2 } });
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
