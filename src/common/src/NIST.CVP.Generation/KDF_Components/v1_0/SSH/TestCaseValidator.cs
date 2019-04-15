using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SSH
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

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.InitialIvClient == null)
            {
                errors.Add($"{nameof(suppliedResult.InitialIvClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.EncryptionKeyClient == null)
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.IntegrityKeyClient == null)
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.InitialIvServer == null)
            {
                errors.Add($"{nameof(suppliedResult.InitialIvServer)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.EncryptionKeyServer == null)
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyServer)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.IntegrityKeyServer == null)
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyServer)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.InitialIvClient.Equals(suppliedResult.InitialIvClient))
            {
                errors.Add($"{nameof(suppliedResult.InitialIvClient)} does not match");
                expected.Add(nameof(_serverTestCase.InitialIvClient), _serverTestCase.InitialIvClient.ToHex());
                provided.Add(nameof(suppliedResult.InitialIvClient), suppliedResult.InitialIvClient.ToHex());
            }

            if (!_serverTestCase.EncryptionKeyClient.Equals(suppliedResult.EncryptionKeyClient))
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyClient)} does not match");
                expected.Add(nameof(_serverTestCase.EncryptionKeyClient), _serverTestCase.EncryptionKeyClient.ToHex());
                provided.Add(nameof(suppliedResult.EncryptionKeyClient), suppliedResult.EncryptionKeyClient.ToHex());
            }

            if (!_serverTestCase.IntegrityKeyClient.Equals(suppliedResult.IntegrityKeyClient))
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyClient)} does not match");
                expected.Add(nameof(_serverTestCase.IntegrityKeyClient), _serverTestCase.IntegrityKeyClient.ToHex());
                provided.Add(nameof(suppliedResult.IntegrityKeyClient), suppliedResult.IntegrityKeyClient.ToHex());
            }

            if (!_serverTestCase.InitialIvServer.Equals(suppliedResult.InitialIvServer))
            {
                errors.Add($"{nameof(suppliedResult.InitialIvServer)} does not match");
                expected.Add(nameof(_serverTestCase.InitialIvServer), _serverTestCase.InitialIvServer.ToHex());
                provided.Add(nameof(suppliedResult.InitialIvServer), suppliedResult.InitialIvServer.ToHex());
            }

            if (!_serverTestCase.EncryptionKeyServer.Equals(suppliedResult.EncryptionKeyServer))
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyServer)} does not match");
                expected.Add(nameof(_serverTestCase.EncryptionKeyServer), _serverTestCase.EncryptionKeyServer.ToHex());
                provided.Add(nameof(suppliedResult.EncryptionKeyServer), suppliedResult.EncryptionKeyServer.ToHex());
            }

            if (!_serverTestCase.IntegrityKeyServer.Equals(suppliedResult.IntegrityKeyServer))
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyServer)} does not match");
                expected.Add(nameof(_serverTestCase.IntegrityKeyServer), _serverTestCase.IntegrityKeyServer.ToHex());
                provided.Add(nameof(suppliedResult.IntegrityKeyServer), suppliedResult.IntegrityKeyServer.ToHex());
            }
        }
    }
}
