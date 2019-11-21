using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF.v1_0
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, KdfResult> _deferredTestCaseResolver;
        private readonly TestGroup _group;

        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase, TestGroup group,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, KdfResult> resolver)
        {
            _serverTestCase = serverTestCase;
            _group = group;
            _deferredTestCaseResolver = resolver;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase iutResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(iutResult, _group, errors);
            if (errors.Count == 0)
            {
                await CheckResults(iutResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, TestGroup group, List<string> errors)
        {
            if (suppliedResult.KeyOut == null)
            {
                errors.Add($"{nameof(suppliedResult.KeyOut)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.FixedData == null)
            {
                errors.Add($"{nameof(suppliedResult.FixedData)} was not present in the {nameof(TestCase)}");
                return;
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (_group.KdfMode == KdfModes.Counter && _group.CounterLocation == CounterLocations.MiddleFixedData)
            {
                if (suppliedResult.BreakLocation <= 0 || suppliedResult.BreakLocation >= suppliedResult.FixedData.BitLength)
                {
                    errors.Add($"Break location is outside the range of {nameof(CounterLocations.MiddleFixedData)}");
                    return;
                }
            }

            try
            {
                var serverResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_group, _serverTestCase, suppliedResult);

                // Check the less than so 'GetMostSignificantBits' doesn't cause an error
                if (suppliedResult.KeyOut.BitLength < _group.KeyOutLength)
                {
                    errors.Add("KeyOut does not match");
                    expected.Add(nameof(serverResult.KeyOut), serverResult.KeyOut.ToHex());
                    provided.Add(nameof(suppliedResult.KeyOut), suppliedResult.KeyOut.ToHex());
                    return;
                }

                if (!serverResult.KeyOut.Equals(suppliedResult.KeyOut.GetMostSignificantBits(_group.KeyOutLength)))
                {
                    errors.Add("KeyOut does not match");
                    expected.Add(nameof(serverResult.KeyOut), serverResult.KeyOut.ToHex());
                    provided.Add(nameof(suppliedResult.KeyOut), suppliedResult.KeyOut.ToHex());
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                errors.Add($"Server unable to complete test case with error: {ex.Message}");
                return;
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
