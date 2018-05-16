using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, KdfResult> _deferredTestCaseResolver;
        private readonly TestGroup _group;

        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase, TestGroup group,
            IDeferredTestCaseResolver<TestGroup, TestCase, KdfResult> resolver)
        {
            _serverTestCase = serverTestCase;
            _group = group;
            _deferredTestCaseResolver = resolver;
        }

        public TestCaseValidation Validate(TestCase iutResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(iutResult, _group, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _serverTestCase, suppliedResult);

            if (_group.KdfMode == KdfModes.Counter && _group.CounterLocation == CounterLocations.MiddleFixedData)
            {
                if (suppliedResult.BreakLocation <= 0 || suppliedResult.BreakLocation >= suppliedResult.FixedData.BitLength)
                {
                    errors.Add($"Break location is outside the range of {nameof(CounterLocations.MiddleFixedData)}");
                    return;
                }
            }

            if (!serverResult.Success)
            {
                errors.Add($"Server unable to complete test case with error: {serverResult.ErrorMessage}");
                return;
            }

            // Check the less than so 'GetMostSignificantBits' doesn't cause an error
            if (suppliedResult.KeyOut.BitLength < _group.KeyOutLength)
            {
                errors.Add("KeyOut does not match");
                expected.Add(nameof(serverResult.DerivedKey), serverResult.DerivedKey.ToHex());
                provided.Add(nameof(suppliedResult.KeyOut), suppliedResult.KeyOut.ToHex());
                return;
            }

            if (!serverResult.DerivedKey.Equals(suppliedResult.KeyOut.GetMostSignificantBits(_group.KeyOutLength)))
            {
                errors.Add("KeyOut does not match");
                expected.Add(nameof(serverResult.DerivedKey), serverResult.DerivedKey.ToHex());
                provided.Add(nameof(suppliedResult.KeyOut), suppliedResult.KeyOut.ToHex());
            }
        }
    }
}
