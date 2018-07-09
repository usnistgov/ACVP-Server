using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Helpers;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorAft : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _serverGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KeyResult> _deferredTestCaseResolver;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorAft(TestCase expectedResult, TestGroup serverGroup, IDeferredTestCaseResolver<TestGroup, TestCase, KeyResult> deferredTestCaseResolver)
        {
            _expectedResult = expectedResult;
            _serverGroup = serverGroup;
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (_expectedResult.Deferred)
            {
                var computedResult = _deferredTestCaseResolver.CompleteDeferredCrypto(_serverGroup, _expectedResult, suppliedResult);
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
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                {
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.Key.PrivKey.P.Equals(suppliedResult.Key.PrivKey.P))
            {
                errors.Add("P does not match");
                expected.Add(nameof(_expectedResult.Key.PrivKey.P), _expectedResult.Key.PrivKey.P.ToHex());
                provided.Add(nameof(suppliedResult.Key.PrivKey.P), suppliedResult.Key.PrivKey.P.ToHex());
            }

            if (!_expectedResult.Key.PrivKey.Q.Equals(suppliedResult.Key.PrivKey.Q))
            {
                errors.Add("Q does not match");
                expected.Add(nameof(_expectedResult.Key.PrivKey.Q), _expectedResult.Key.PrivKey.Q.ToHex());
                provided.Add(nameof(suppliedResult.Key.PrivKey.Q), suppliedResult.Key.PrivKey.Q.ToHex());
            }

            if (!_expectedResult.Key.PubKey.N.Equals(suppliedResult.Key.PubKey.N))
            {
                errors.Add("N does not match");
                expected.Add(nameof(_expectedResult.Key.PubKey.N), _expectedResult.Key.PubKey.N.ToHex());
                provided.Add(nameof(suppliedResult.Key.PubKey.N), suppliedResult.Key.PubKey.N.ToHex());
            }

            if (_serverGroup.KeyFormat == PrivateKeyModes.Standard)
            {
                if (suppliedResult.Key.PrivKey is PrivateKey standardKey)
                {
                    // Assuming that _serverGroup.KeyFormat matches the actual type
                    if (_expectedResult.Key.PrivKey is PrivateKey expectedKey)
                    {
                        if (!expectedKey.D.Equals(standardKey.D))
                        {
                            errors.Add("D does not match");
                            expected.Add(nameof(expectedKey.D), expectedKey.D.ToHex());
                            provided.Add(nameof(standardKey.D), standardKey.D.ToHex());
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
            else if (_serverGroup.KeyFormat == PrivateKeyModes.Crt)
            {
                if (suppliedResult.Key.PrivKey is CrtPrivateKey crtKey)
                {
                    // Assuming that _serverGroup.KeyFormat matches the actual type
                    if (_expectedResult.Key.PrivKey is CrtPrivateKey expectedKey)
                    {
                        if (!expectedKey.DMP1.Equals(crtKey.DMP1))
                        {
                            errors.Add("DMP1 does not match");
                            expected.Add(nameof(expectedKey.DMP1), expectedKey.DMP1.ToHex());
                            provided.Add(nameof(crtKey.DMP1), crtKey.DMP1.ToHex()); 
                        }

                        if (!expectedKey.DMQ1.Equals(crtKey.DMQ1))
                        {
                            errors.Add("DMQ1 does not match");
                            expected.Add(nameof(expectedKey.DMQ1), expectedKey.DMQ1.ToHex());
                            provided.Add(nameof(crtKey.DMQ1), crtKey.DMQ1.ToHex());
                        }

                        if (!expectedKey.IQMP.Equals(crtKey.IQMP))
                        {
                            errors.Add("IQMP does not match");
                            expected.Add(nameof(expectedKey.IQMP), expectedKey.IQMP.ToHex());
                            provided.Add(nameof(crtKey.IQMP), crtKey.IQMP.ToHex());
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
