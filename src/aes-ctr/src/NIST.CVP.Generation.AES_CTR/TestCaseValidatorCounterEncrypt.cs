using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorCounterEncrypt : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult> _deferredTestCaseResolver;
        private readonly TestCase _serverTestCase;
        private readonly TestGroup _group;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidatorCounterEncrypt(TestGroup group, TestCase testCase, IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult> resolver)
        {
            _serverTestCase = testCase;
            _deferredTestCaseResolver = resolver;
            _group = group;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                var calculatedIVs = CheckResults(suppliedResult, errors, expected, provided);
                ValidateIVs(calculatedIVs, errors);
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
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
            }

            // no longer required
            /*if (suppliedResult.IVs == null)
            {
                errors.Add($"{nameof(suppliedResult.IVs)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.IVs.Count != _serverTestCase.PlainText.BitLength / 128)
            {
                errors.Add($"{nameof(suppliedResult.IVs)} does not have the correct number of values");
            }*/
        }

        private List<BitString> CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _serverTestCase, suppliedResult);

            if (!serverResult.Success)
            {
                errors.Add($"Server unable to complete test case with error: {serverResult.ErrorMessage}");
                return new List<BitString>();
            }

            if (!serverResult.Result.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
                expected.Add(nameof(_serverTestCase.CipherText), _serverTestCase.CipherText.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }

            return serverResult.IVs;
        }

        private void ValidateIVs(List<BitString> ivs, List<string> errors)
        {
            var expectedBreaks = _group.OverflowCounter ? 1 : 0;
            var ivValues = ivs.Select(iv => iv.ToPositiveBigInteger()).ToList();

            // Check for distinctness
            if (ivValues.Count != ivValues.Distinct().Count())
            {
                errors.Add("IVs are not distinct");
                return;
            }

            // Check for sequential order
            var actualBreaks = 0;
            var breakLocation = 0;
            for (var i = 0; i < ivValues.Count - 1; i++)
            {
                if (_group.IncrementalCounter)
                {
                    if (ivValues[i] > ivValues[i + 1])
                    {
                        actualBreaks++;
                        breakLocation = i + 1;

                        if (actualBreaks > expectedBreaks)
                        {
                            errors.Add($"Unexpected counter overflow at IV {i}");
                            return;
                        }
                    }
                }
                else
                {
                    if (ivValues[i] < ivValues[i + 1])
                    {
                        actualBreaks++;
                        breakLocation = i + 1;

                        if (actualBreaks > expectedBreaks)
                        {
                            errors.Add($"Unexpected counter underflow at IV {i}");
                            return;
                        }
                    }
                }
            }

            if (actualBreaks != expectedBreaks)
            {
                errors.Add("Expected overflow/underflow but none occurred");
                return;
            }

            var sortedIvValues = new List<BigInteger>();
            for (var i = breakLocation; i < ivValues.Count; i++)
            {
                sortedIvValues.Add(ivValues[i]);
            }

            for (var i = 0; i < breakLocation; i++)
            {
                sortedIvValues.Add(ivValues[i]);
            }

            for (var i = 0; i < sortedIvValues.Count - 1; i++)
            {
                if (_group.IncrementalCounter)
                {
                    if (sortedIvValues[i] > sortedIvValues[i + 1])
                    {
                        errors.Add($"IV {i} is greater than IV {i + 1}");
                        return;
                    }
                }
                else
                {
                    if (sortedIvValues[i] < sortedIvValues[i + 1])
                    {
                        errors.Add($"IV {i} is less than IV {i + 1}");
                        return;
                    }
                }
            }
        }
    }
}
