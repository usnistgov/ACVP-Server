using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0
{
    public class TestCaseValidatorCounterEncrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, SymmetricCounterResult> _deferredTestCaseResolver;
        private readonly TestCase _serverTestCase;
        private readonly TestGroup _group;
        private List<BitString> _ivs = new List<BitString>();
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidatorCounterEncrypt(
            TestGroup group,
            TestCase testCase,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, SymmetricCounterResult> resolver)
        {
            _serverTestCase = testCase;
            _deferredTestCaseResolver = resolver;
            _group = group;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                await CheckResults(suppliedResult, errors, expected, provided);
                ValidateIVs(_ivs, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
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
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_group, _serverTestCase, suppliedResult);

            if (!serverResult.Success)
            {
                errors.Add($"Server unable to complete test case with error: {serverResult.ErrorMessage}");
                return;
            }

            // only check first block
            if (!serverResult.Result.GetMostSignificantBits(64).Equals(suppliedResult.CipherText.GetMostSignificantBits(64)))     // 64 is block size
            {
                errors.Add("Cipher Text does not match");
                expected.Add(nameof(serverResult.Result), serverResult.Result.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }

            _ivs = serverResult.IVs;
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
