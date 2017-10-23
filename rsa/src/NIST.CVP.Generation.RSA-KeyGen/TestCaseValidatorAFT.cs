using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorAFT : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorAFT(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            var crtForm = ValidateResultsPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                if (crtForm)
                {
                    CheckCRTResults(suppliedResult, errors);
                }
                else
                {
                    CheckStandardResults(suppliedResult, errors);
                }

                CheckCommonResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors)};
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed};
        }

        private bool ValidateResultsPresent(TestCase suppliedResult, List<string> errors)
        {
            var crtForm = false;

            if (suppliedResult == null)
            {
                errors.Add($"{nameof(suppliedResult)} is null");
                return false;
            }

            if (suppliedResult.Key.PrivKey == null)
            {
                errors.Add($"{nameof(suppliedResult.Key.PrivKey)} not present in {nameof(TestCase)}");
                return false;
            }

            if (suppliedResult.Key.PrivKey.D == 0)
            {
                if (suppliedResult.Key.PrivKey.DMP1 == 0 || suppliedResult.Key.PrivKey.DMQ1 == 0 ||
                    suppliedResult.Key.PrivKey.IQMP == 0)
                {
                    errors.Add($"{nameof(suppliedResult.Key.PrivKey)} exponent values were not present in {nameof(TestCase)}");
                    crtForm = false;
                }
                else
                {
                    // We have CRT form and all CRT content
                    crtForm = true;
                }
            }
            else
            {
                // We have standard form and all standard content
                crtForm = false;
            }

            // Check for N, P, Q which should always be present
            if (suppliedResult.Key.PubKey.N == 0)
            {
                errors.Add($"{nameof(suppliedResult.Key.PubKey.N)} value not present in {nameof(TestCase)}");
            }

            if (suppliedResult.Key.PrivKey.P == 0 || suppliedResult.Key.PrivKey.Q == 0)
            {
                errors.Add($"{nameof(suppliedResult.Key.PrivKey)} prime values were not present in {nameof(TestCase)}");
            }

            return crtForm;
        }

        private void CheckCRTResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Key.PrivKey.DMP1.Equals(suppliedResult.Key.PrivKey.DMP1))
            {
                errors.Add("DMP1 does not match");   
            }

            if (!_expectedResult.Key.PrivKey.DMQ1.Equals(suppliedResult.Key.PrivKey.DMQ1))
            {
                errors.Add("DMQ1 does not match");
            }

            if (!_expectedResult.Key.PrivKey.IQMP.Equals(suppliedResult.Key.PrivKey.IQMP))
            {
                errors.Add("IQMP does not match");
            }
        }

        private void CheckStandardResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Key.PrivKey.D.Equals(suppliedResult.Key.PrivKey.D))
            {
                errors.Add("D does not match");
            }
        }

        private void CheckCommonResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Key.PrivKey.P.Equals(suppliedResult.Key.PrivKey.P))
            {
                errors.Add("P does not match");
            }

            if (!_expectedResult.Key.PrivKey.Q.Equals(suppliedResult.Key.PrivKey.Q))
            {
                errors.Add("Q does not match");
            }

            if (!_expectedResult.Key.PubKey.N.Equals(suppliedResult.Key.PubKey.N))
            {
                errors.Add("N does not match");
            }
        }
    }
}
