using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestCaseValidatorDeferredRfcEncrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesResult> _deferredTestCaseResolver;
        private readonly TestCase _serverTestCase;
        private readonly TestGroup _group;

        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidatorDeferredRfcEncrypt(
            TestGroup @group,
            TestCase serverTestCase,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesResult> deferredTestCaseResolver)
        {
            _group = group;
            _serverTestCase = serverTestCase;
            _deferredTestCaseResolver = deferredTestCaseResolver;
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
            if (suppliedResult.IV == null)
            {
                errors.Add($"{nameof(suppliedResult.IV)} was not present in the {nameof(TestCase)}");
            }
            else
            {
                if (suppliedResult.IV.BitLength != 128)
                {
                    errors.Add($"{nameof(suppliedResult.IV)} did not contain the correct amount of bits.  Expected 128, got {suppliedResult.IV.BitLength}");
                }

                if (suppliedResult.IV.BitLength >= 32)
                {
                    var lsb = new BitString(suppliedResult.IV.GetLeastSignificantBits(32).Bits).ToPositiveBigInteger();

                    if (lsb != 1)
                    {
                        errors.Add($"expected {nameof(suppliedResult.IV)} to have an integer value of 1 in the 32 least significant bits.");
                    }
                }
            }

            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            try
            {
                var serverResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_group, _serverTestCase, suppliedResult);

                if (!serverResult.CipherText.ToHex().Equals(suppliedResult.CipherText.ToHex()))
                {
                    errors.Add("Cipher Text does not match");
                    expected.Add(nameof(serverResult.CipherText), serverResult.CipherText.ToHex());
                    provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
                }
            }
            catch (Exception e)
            {
                ThisLogger.Error(e);
                errors.Add($"Server unable to complete test case with error: {e.Message}");
            }
        }

        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();
    }
}
