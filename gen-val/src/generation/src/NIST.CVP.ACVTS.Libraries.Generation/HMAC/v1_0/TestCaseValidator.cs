using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _currentGroup;

        public TestCaseValidator(TestCase expectedResult, TestGroup currentGroup)
        {
            _expectedResult = expectedResult;
            _currentGroup = currentGroup;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

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

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Mac == null)
            {
                errors.Add($"{nameof(suppliedResult.Mac)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            SetBitStringLengthsToCorrectLength(suppliedResult);

            if (!_expectedResult.Mac.Equals(suppliedResult.Mac))
            {
                errors.Add("MAC does not match");
                expected.Add(nameof(_expectedResult.Mac), _expectedResult.Mac.ToHex());
                provided.Add(nameof(suppliedResult.Mac), suppliedResult.Mac.ToHex());
            }
        }

        /// <summary>
        /// The <see cref="TestCase"/> <see cref="BitString"/>s are not parsed w/ the <see cref="TestGroups"/> length in mind.
        /// This method will ensure the correct bitlengths are compared.
        /// </summary>
        /// <param name="suppliedResult"></param>
        private void SetBitStringLengthsToCorrectLength(TestCase suppliedResult)
        {
            _expectedResult.Mac = GetBitStringAtLengthOrOriginalIfNotLongEnough(_expectedResult.Mac, _currentGroup.MacLength);
            suppliedResult.Mac = GetBitStringAtLengthOrOriginalIfNotLongEnough(suppliedResult.Mac, _currentGroup.MacLength);
        }

        private BitString GetBitStringAtLengthOrOriginalIfNotLongEnough(BitString bs, int intendedLength)
        {
            if (bs.BitLength < intendedLength)
            {
                return bs;
            }

            return bs.GetMostSignificantBits(intendedLength);
        }
    }
}
