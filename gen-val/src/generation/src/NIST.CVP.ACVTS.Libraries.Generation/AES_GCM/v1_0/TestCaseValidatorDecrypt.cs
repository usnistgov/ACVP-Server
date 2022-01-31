using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0
{
    public class TestCaseValidatorDecrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestGroup _testGroup;
        private readonly TestCase _expectedResult;

        public TestCaseValidatorDecrypt(TestGroup testGroup, TestCase expectedResult)
        {
            _testGroup = testGroup;
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (_testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0 && _expectedResult.TestPassed != null && !_expectedResult.TestPassed.Value)
            {
                if (!suppliedResult.TestPassed.HasValue ||
                    (suppliedResult.TestPassed != null && suppliedResult.TestPassed.Value))
                {
                    errors.Add("Expected tag validation failure");
                    expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());
                    provided.Add(nameof(suppliedResult.TestPassed),
                        suppliedResult.TestPassed.HasValue ? suppliedResult.TestPassed.ToString() : "none");
                }
            }
            else
            {
                ValidateResultPresent(suppliedResult, errors);
                if (errors.Count == 0)
                {
                    CheckResults(suppliedResult, errors, expected, provided);
                }
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0 && suppliedResult.PlainText == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (_testGroup.AlgoMode == AlgoMode.AES_GMAC_v1_0 && suppliedResult.TestPassed == null)
            {
                errors.Add($"{nameof(suppliedResult.TestPassed)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0 && !_expectedResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add("Plain Text does not match");
                expected.Add(nameof(_expectedResult.PlainText), _expectedResult.PlainText.ToHex());
                provided.Add(nameof(suppliedResult.PlainText), suppliedResult.PlainText.ToHex());
            }

            if (_testGroup.AlgoMode == AlgoMode.AES_GMAC_v1_0 && !_expectedResult.TestPassed.Equals(suppliedResult.TestPassed))
            {
                errors.Add("TestPassed does not match");
                expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.ToString());
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.ToString());
            }
        }
    }
}
