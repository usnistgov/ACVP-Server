using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public class TestCaseValidatorVal<TTestGroup, TTestCase, TKeyPair> : ITestCaseValidatorAsync<TTestGroup, TTestCase>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TKeyPair : IDsaKeyPair
	{
		private readonly TTestCase _expectedResult;

        public TestCaseValidatorVal(TTestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TTestCase suppliedResult, bool showExpected = false)
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
                return await Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return await Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.TestPassed == null)
            {
                errors.Add($"{nameof(suppliedResult.TestPassed)} was not present in the {nameof(TTestCase)}");
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                errors.Add($"Incorrect {nameof(suppliedResult.TestPassed)} result. Test expectation: \"{EnumHelpers.GetEnumDescriptionFromEnum(_expectedResult.TestCaseDisposition)}\"");
                expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString());
            }
        }
	}
}