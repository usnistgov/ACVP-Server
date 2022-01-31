using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3
{
    public class TestCaseValidatorAft<TTestGroup, TTestCase, TKeyPair> : ITestCaseValidatorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        private readonly TTestCase _workingTest;
        private readonly TTestGroup _testGroup;
        private readonly IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult> _deferredResolver;

        public TestCaseValidatorAft(TTestCase workingTest,
            TTestGroup testGroup,
            IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult> deferredResolver)
        {
            _workingTest = workingTest;
            _testGroup = testGroup;
            _deferredResolver = deferredResolver;
        }

        public int TestCaseId => _workingTest.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TTestCase suppliedResult, bool showExpected = false)
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

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                ValidatePublicKeyComponent(suppliedResult.StaticKeyIut, nameof(suppliedResult.StaticKeyIut), errors);
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesEphemeralKeyPair)
            {
                ValidatePublicKeyComponent(suppliedResult.EphemeralKeyIut, nameof(suppliedResult.EphemeralKeyIut), errors);
            }

            // When a hash function does not exist, check z directly, otherwise check the hash of z
            if (_testGroup.HashFunctionZ == HashFunctions.None)
            {
                if (suppliedResult.Z == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Z)} but was not supplied");
                }
            }
            else
            {
                if (suppliedResult.HashZ == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
                }
            }
        }

        private void ValidatePublicKeyComponent(TKeyPair keyPair, string keyLabel, List<string> errors)
        {
            switch (keyPair)
            {
                case EccKeyPair ecc:
                    if (ecc.PublicQ.X == 0)
                    {
                        errors.Add($"Expected {keyLabel}.X but was not supplied");
                    }

                    if (ecc.PublicQ.Y == 0)
                    {
                        errors.Add($"Expected {keyLabel}.Y but was not supplied");
                    }
                    break;
                case FfcKeyPair ffc:
                    if (ffc.PublicKeyY == 0)
                    {
                        errors.Add($"Expected {keyLabel} but was not supplied");
                    }
                    break;
            }
        }

        private async Task CheckResults(
            TTestCase suppliedResult,
            List<string> errors,
            Dictionary<string, string> expected,
            Dictionary<string, string> provided)
        {
            var serverResult = await _deferredResolver.CompleteDeferredCryptoAsync(
                _testGroup, _workingTest, suppliedResult
            );

            if (_testGroup.HashFunctionZ == HashFunctions.None)
            {
                if (!serverResult.Z.Equals(suppliedResult.Z))
                {
                    errors.Add($"{nameof(suppliedResult.Z)} does not match");
                    expected.Add(nameof(serverResult.Z), serverResult.Z.ToHex());
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
