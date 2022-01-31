using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase iutResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(iutResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Passed });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ClientEarlyTrafficSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ClientEarlyTrafficSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.EarlyExporterMasterSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.EarlyExporterMasterSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ClientHandshakeTrafficSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ClientHandshakeTrafficSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ServerHandshakeTrafficSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ServerHandshakeTrafficSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ClientApplicationTrafficSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ClientApplicationTrafficSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ServerApplicationTrafficSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ServerApplicationTrafficSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ExporterMasterSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ExporterMasterSecret)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.ResumptionMasterSecret == null)
            {
                errors.Add($"{nameof(suppliedResult.ResumptionMasterSecret)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.ClientEarlyTrafficSecret.Equals(suppliedResult.ClientEarlyTrafficSecret))
            {
                errors.Add($"{nameof(suppliedResult.ClientEarlyTrafficSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ClientEarlyTrafficSecret), _serverTestCase.ClientEarlyTrafficSecret.ToHex());
                provided.Add(nameof(suppliedResult.ClientEarlyTrafficSecret), suppliedResult.ClientEarlyTrafficSecret.ToHex());
            }

            if (!_serverTestCase.EarlyExporterMasterSecret.Equals(suppliedResult.EarlyExporterMasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.EarlyExporterMasterSecret)} does not match");
                expected.Add(nameof(_serverTestCase.EarlyExporterMasterSecret), _serverTestCase.EarlyExporterMasterSecret.ToHex());
                provided.Add(nameof(suppliedResult.EarlyExporterMasterSecret), suppliedResult.EarlyExporterMasterSecret.ToHex());
            }

            if (!_serverTestCase.ClientHandshakeTrafficSecret.Equals(suppliedResult.ClientHandshakeTrafficSecret))
            {
                errors.Add($"{nameof(suppliedResult.ClientHandshakeTrafficSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ClientHandshakeTrafficSecret), _serverTestCase.ClientHandshakeTrafficSecret.ToHex());
                provided.Add(nameof(suppliedResult.ClientHandshakeTrafficSecret), suppliedResult.ClientHandshakeTrafficSecret.ToHex());
            }

            if (!_serverTestCase.ServerHandshakeTrafficSecret.Equals(suppliedResult.ServerHandshakeTrafficSecret))
            {
                errors.Add($"{nameof(suppliedResult.ServerHandshakeTrafficSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ServerHandshakeTrafficSecret), _serverTestCase.ServerHandshakeTrafficSecret.ToHex());
                provided.Add(nameof(suppliedResult.ServerHandshakeTrafficSecret), suppliedResult.ServerHandshakeTrafficSecret.ToHex());
            }

            if (!_serverTestCase.ClientApplicationTrafficSecret.Equals(suppliedResult.ClientApplicationTrafficSecret))
            {
                errors.Add($"{nameof(suppliedResult.ClientApplicationTrafficSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ClientApplicationTrafficSecret), _serverTestCase.ClientApplicationTrafficSecret.ToHex());
                provided.Add(nameof(suppliedResult.ClientApplicationTrafficSecret), suppliedResult.ClientApplicationTrafficSecret.ToHex());
            }

            if (!_serverTestCase.ServerApplicationTrafficSecret.Equals(suppliedResult.ServerApplicationTrafficSecret))
            {
                errors.Add($"{nameof(suppliedResult.ServerApplicationTrafficSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ServerApplicationTrafficSecret), _serverTestCase.ServerApplicationTrafficSecret.ToHex());
                provided.Add(nameof(suppliedResult.ServerApplicationTrafficSecret), suppliedResult.ServerApplicationTrafficSecret.ToHex());
            }

            if (!_serverTestCase.ExporterMasterSecret.Equals(suppliedResult.ExporterMasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.ExporterMasterSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ExporterMasterSecret), _serverTestCase.ExporterMasterSecret.ToHex());
                provided.Add(nameof(suppliedResult.ExporterMasterSecret), suppliedResult.ExporterMasterSecret.ToHex());
            }

            if (!_serverTestCase.ResumptionMasterSecret.Equals(suppliedResult.ResumptionMasterSecret))
            {
                errors.Add($"{nameof(suppliedResult.ResumptionMasterSecret)} does not match");
                expected.Add(nameof(_serverTestCase.ResumptionMasterSecret), _serverTestCase.ResumptionMasterSecret.ToHex());
                provided.Add(nameof(suppliedResult.ResumptionMasterSecret), suppliedResult.ResumptionMasterSecret.ToHex());
            }
        }
    }
}
