using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.Enums;

namespace NIST.CVP.Generation.KAS.FFC.Helpers
{
    /// <summary>
    /// Can be used for creating specific errors (or conditions) within a test case
    /// </summary>
    public static class TestCaseDispositionHelper
    {
        #region MangleKeys
        /// <summary>
        /// Used to mangle a private or public key, based on the <see cref="TestCaseDispositionOption"/>
        /// </summary>
        /// <param name="testCase">The current test case</param>
        /// <param name="dispositionOption">The disposition to </param>
        /// <param name="serverKas"></param>
        /// <param name="iutKas"></param>
        public static void MangleKeys(
            TestCase testCase,
            TestCaseDispositionOption dispositionOption, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas
        )
        {
            var serverKeyExpectations = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                serverKas.Scheme.SchemeParameters.KasDsaAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                serverKas.Scheme.SchemeParameters.KeyAgreementRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );
            var iutKeyExpectations = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                iutKas.Scheme.SchemeParameters.KasDsaAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                iutKas.Scheme.SchemeParameters.KeyAgreementRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );

            switch (dispositionOption)
            {
                case TestCaseDispositionOption.FailAssuranceServerStaticPublicKey:
                    MangleServerStaticPublicKey(testCase, serverKas, serverKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey:
                    MangleServerEphemeralPublicKey(testCase, serverKas, serverKeyExpectations.GeneratesEphemeralKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceIutStaticPrivateKey:
                    MangleIutStaticPrivateKey(testCase, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.FailAssuranceIutStaticPublicKey:
                    MangleIutStaticPublicKey(testCase, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case TestCaseDispositionOption.Success:
                case TestCaseDispositionOption.SuccessLeadingZeroNibbleZ:
                case TestCaseDispositionOption.SuccessLeadingZeroNibbleDkm:
                case TestCaseDispositionOption.FailChangedZ:
                case TestCaseDispositionOption.FailChangedDkm:
                case TestCaseDispositionOption.FailChangedOi:
                case TestCaseDispositionOption.FailChangedMacData:
                case TestCaseDispositionOption.FailChangedTag:
                    // These are not key mangling dispositions, do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dispositionOption), dispositionOption, null);
            }
        }

        private static void MangleServerStaticPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.StaticKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.P, 
                        serverKas.Scheme.DomainParameters.Q, 
                        serverKas.Scheme.StaticKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleServerEphemeralPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the ephemeral public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.EphemeralKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.P, 
                        serverKas.Scheme.DomainParameters.Q, 
                        serverKas.Scheme.EphemeralKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleIutStaticPrivateKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static private key to make it invalid
                iutKas.Scheme.StaticKeyPair.PrivateKeyX += 2;
            }
        }

        private static void MangleIutStaticPublicKey(
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    iutKas.Scheme.StaticKeyPair.PublicKeyY += 2;
                    if (!KeyValidationHelper.PerformFfcPublicKeyValidation(
                        iutKas.Scheme.DomainParameters.P, 
                        iutKas.Scheme.DomainParameters.Q, 
                        iutKas.Scheme.StaticKeyPair.PublicKeyY))
                    {
                        break;
                    }
                }
            }
        }
        #endregion MangleKeys

        public static void SetTestCaseInformationFromKasResults(
            TestGroup group, 
            TestCase testCase, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas, 
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas, 
            KasResult iutResult
        )
        {
            testCase.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyServer = serverKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyServer = serverKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;
            testCase.DkmNonceServer = serverKas.Scheme.DkmNonce;
            testCase.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            testCase.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateKeyX ?? 0;
            testCase.StaticPublicKeyIut = iutKas?.Scheme?.StaticKeyPair?.PublicKeyY ?? 0;
            testCase.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateKeyX ?? 0;
            testCase.EphemeralPublicKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PublicKeyY ?? 0;
            testCase.DkmNonceIut = iutKas?.Scheme?.DkmNonce;
            testCase.EphemeralNonceIut = iutKas?.Scheme?.EphemeralNonce;

            testCase.Z = iutResult?.Z;
            testCase.OtherInfo = iutResult?.Oi;
            testCase.OiLen = testCase.OtherInfo?.BitLength ?? 0;
            testCase.Dkm = iutResult?.Dkm;
            testCase.MacData = iutResult?.MacData;
            if (group.KasMode == KasMode.NoKdfNoKc)
            {
                testCase.HashZ = iutResult?.Tag;
            }
            else
            {
                testCase.Tag = iutResult?.Tag;
            }
        }
    }
}