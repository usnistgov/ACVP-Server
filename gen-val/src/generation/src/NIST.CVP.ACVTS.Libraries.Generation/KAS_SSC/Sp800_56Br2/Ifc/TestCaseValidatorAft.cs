using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasSscAftDeferredResultIfc> _deferredTestCaseResolverAsync;

        public TestCaseValidatorAft(
            TestCase workingTest,
            TestGroup testGroup,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasSscAftDeferredResultIfc> deferredTestCaseResolverAsync)
        {
            _workingTest = workingTest;
            _testGroup = testGroup;
            _deferredTestCaseResolverAsync = deferredTestCaseResolverAsync;
        }

        public int TestCaseId => _workingTest.TestCaseId;

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
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {

            if (_testGroup.HashFunctionZ == HashFunctions.None)
            {
                if (suppliedResult.Z == null || suppliedResult.Z.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Z)} but was not supplied");
                }
            }
            else
            {
                if (suppliedResult.HashZ == null || suppliedResult.HashZ.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
                }
            }

            if (ShouldSupplyValueC())
            {
                if (suppliedResult.IutC == null || suppliedResult.IutC.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.IutC)} but was not supplied");
                }
            }
        }

        /// <summary>
        /// C value should be provided when IUT is kas1 party u, kas2.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyValueC()
        {
            var map = new List<(SscIfcScheme scheme, KeyAgreementRole role)>()
            {
                (SscIfcScheme.Kas1, KeyAgreementRole.InitiatorPartyU),
                (SscIfcScheme.Kas2, KeyAgreementRole.InitiatorPartyU),
                (SscIfcScheme.Kas2, KeyAgreementRole.ResponderPartyV),
            };

            return map.TryFirst(w => w.scheme == _testGroup.Scheme && w.role == _testGroup.KasRole, out var result);
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = await _deferredTestCaseResolverAsync.CompleteDeferredCryptoAsync(
                _testGroup, _workingTest, suppliedResult
            );

            if (!serverResult.Success)
            {
                errors.Add($"Failed completing deferred crypto. {serverResult.ErrorMessage}");
                return;
            }

            if (_testGroup.HashFunctionZ == HashFunctions.None)
            {
                if (!serverResult.Result.Z.Equals(suppliedResult.Z))
                {
                    errors.Add($"{nameof(suppliedResult.Z)} does not match");
                    expected.Add(nameof(serverResult.Result.Z), serverResult.Result.Z.ToHex());
                    provided.Add(nameof(suppliedResult.Z), suppliedResult.Z.ToHex());
                }
            }
            else
            {
                if (!serverResult.HashZ.Equals(suppliedResult.HashZ))
                {
                    errors.Add($"{nameof(suppliedResult.HashZ)} does not match");
                    expected.Add(nameof(serverResult.HashZ), serverResult.HashZ.ToHex());
                    provided.Add(nameof(suppliedResult.HashZ), suppliedResult.HashZ.ToHex());
                }
            }
        }
    }
}
