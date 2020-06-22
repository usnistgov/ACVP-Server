using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using Orleans.Runtime;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
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

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesDkmNonce)
            {
                if (suppliedResult.DkmNonceIut == null || suppliedResult.DkmNonceIut.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.DkmNonceIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesEphemeralNonce)
            {
                if (suppliedResult.EphemeralNonceIut == null || suppliedResult.EphemeralNonceIut.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralNonceIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyConfirmationDirection != KeyConfirmationDirection.None)
            {
                if (suppliedResult.Tag == null)
                {
                    errors.Add($"Expected {nameof(suppliedResult.Tag)} but was not supplied");
                }                
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

            if (!serverResult.Success)
            {
                errors.Add($"Failed completing deferred crypto. {serverResult.ErrorMessage}");
                return;
            }
            
            if (!serverResult.Dkm.Equals(suppliedResult.Dkm))
            {
                errors.Add($"{nameof(suppliedResult.Dkm)} does not match");
                expected.Add(nameof(serverResult.Dkm), serverResult.Dkm.ToHex());
                provided.Add(nameof(suppliedResult.Dkm), suppliedResult.Dkm.ToHex());
            }

            if (_testGroup.KeyConfirmationDirection != KeyConfirmationDirection.None)
            {
                if (!serverResult.MacKey.Equals(_workingTest.MacKey))
                {
                    errors.Add($"{nameof(suppliedResult.MacKey)} does not match");
                    expected.Add(nameof(_workingTest.MacKey), _workingTest.MacKey.ToHex());
                    provided.Add(nameof(serverResult.MacKey), serverResult.MacKey.ToHex());
                }
                
                if (!serverResult.MacData.Equals(_workingTest.MacData))
                {
                    errors.Add($"{nameof(suppliedResult.MacData)} does not match");
                    expected.Add(nameof(_workingTest.MacData), _workingTest.MacData.ToHex());
                    provided.Add(nameof(serverResult.MacData), serverResult.MacData.ToHex());
                }
                
                if (!serverResult.Tag.Equals(suppliedResult.Tag))
                {
                    errors.Add($"{nameof(suppliedResult.Tag)} does not match");
                    expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                    provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
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
    }
}