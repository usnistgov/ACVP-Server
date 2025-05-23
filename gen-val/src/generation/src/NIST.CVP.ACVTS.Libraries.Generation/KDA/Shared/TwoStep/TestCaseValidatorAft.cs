﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared.TwoStep
{
    public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;

        public TestCaseValidatorAft(TestCase workingTest)
        {
            _workingTest = workingTest;
        }

        public int TestCaseId => _workingTest.TestCaseId;

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }
            return Task.FromResult(new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_workingTest.Dkm != null)
            {
                if (suppliedResult.Dkm == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Dkm)} but was not supplied");
                }
            }

            if (_workingTest.Dkms != null)
            {
                if (suppliedResult.Dkms == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Dkms)} but was not supplied");
                    return;
                }

                if (_workingTest.Dkms.Count != suppliedResult.Dkms.Count)
                {
                    errors.Add($"Number of included {nameof(_workingTest.Dkms)} is invalid.  Expected {_workingTest.Dkms.Count} got {suppliedResult.Dkms.Count}");
                }
            }
        }

        private void CheckResults(
            TestCase suppliedResult,
            List<string> errors,
            Dictionary<string, string> expected,
            Dictionary<string, string> provided)
        {
            if (_workingTest.Dkm != null)
            {
                if (!_workingTest.Dkm.Equals(suppliedResult.Dkm))
                {
                    errors.Add($"{nameof(suppliedResult.Dkm)} does not match");
                    expected.Add(nameof(_workingTest.Dkm), _workingTest.Dkm.ToHex());
                    provided.Add(nameof(suppliedResult.Dkm), suppliedResult.Dkm.ToHex());
                }
            }

            if (_workingTest.Dkms != null)
            {
                var index = 0;
                foreach (var dkm in _workingTest.Dkms)
                {
                    if (!dkm.Equals(suppliedResult.Dkms[index]))
                    {
                        errors.Add($"{nameof(suppliedResult.Dkms)}[{index}] does not match.");
                        expected.Add($"{nameof(_workingTest.Dkms)}[{index}]", _workingTest.Dkms[index].ToHex());
                        provided.Add($"{nameof(suppliedResult.Dkms)}[{index}]", suppliedResult.Dkms[index].ToHex());
                    }
                    index++;
                }
            }
        }
    }
}
