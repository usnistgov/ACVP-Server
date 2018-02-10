using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorAft : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KeyResult> _deferredTestCaseResolver;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorAft(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolver<TestGroup, TestCase, KeyResult> deferredTestCaseResolver)
        {
            _expectedResult = expectedResult;
            _group = group;
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (_expectedResult.Deferred)
            {
                var computedResult = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _expectedResult, suppliedResult);
                if (!computedResult.Success)
                {
                    errors.Add($"Unable to resolve deferred crypto: {computedResult.ErrorMessage}");
                }
                else
                {
                    _expectedResult.Key = computedResult.Key;
                }
            }

            ValidateResultsPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors)};
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed};
        }

        private void ValidateResultsPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult == null)
            {
                errors.Add($"{nameof(suppliedResult)} is null");
                return;
            }

            if (suppliedResult.Key.PrivKey == null)
            {
                errors.Add($"{nameof(suppliedResult.Key.PrivKey)} not present in {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.Key.PubKey == null)
            {
                errors.Add($"{nameof(suppliedResult.Key.PubKey)} not present in {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
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

            if (_group.KeyFormat == PrivateKeyModes.Standard)
            {
                if (suppliedResult.Key.PrivKey is PrivateKey standardKey)
                {
                    // Assuming that _group.KeyFormat matches the actual type
                    if (_expectedResult.Key.PrivKey is PrivateKey expectedKey)
                    {
                        if (!expectedKey.D.Equals(standardKey.D))
                        {
                            errors.Add("D does not match");
                        }
                    }
                    else
                    {
                        errors.Add("Internal key is unexpected type");
                    }
                }
                else
                {
                    errors.Add("Unexpected private key format");
                }
            }
            else if (_group.KeyFormat == PrivateKeyModes.Crt)
            {
                if (suppliedResult.Key.PrivKey is CrtPrivateKey crtKey)
                {
                    // Assuming that _group.KeyFormat matches the actual type
                    if (_expectedResult.Key.PrivKey is CrtPrivateKey expectedKey)
                    {
                        if (!expectedKey.DMP1.Equals(crtKey.DMP1))
                        {
                            errors.Add("DMP1 does not match");   
                        }

                        if (!expectedKey.DMQ1.Equals(crtKey.DMQ1))
                        {
                            errors.Add("DMQ1 does not match");
                        }

                        if (!expectedKey.IQMP.Equals(crtKey.IQMP))
                        {
                            errors.Add("IQMP does not match");
                        }
                    }
                    else
                    {
                        errors.Add("Internal key is unexpected type");
                    }
                }
                else
                {
                    errors.Add("Unexpected private key format");
                }
            }
            else
            {
                errors.Add("No key format selected, unable to process test case");
            }
        }
    }
}
