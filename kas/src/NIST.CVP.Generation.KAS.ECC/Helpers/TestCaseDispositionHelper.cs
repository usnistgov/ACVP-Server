using System;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Generation.KAS.Enums;

namespace NIST.CVP.Generation.KAS.ECC.Helpers
{
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
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas
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
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.StaticKeyPair.PublicQ = new EccPoint(
                        serverKas.Scheme.StaticKeyPair.PublicQ.X + 2,
                        serverKas.Scheme.StaticKeyPair.PublicQ.Y
                    );
                    if (!KeyValidationHelper.PerformEccPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.CurveE,
                        serverKas.Scheme.StaticKeyPair.PublicQ,
                        false))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleServerEphemeralPublicKey(
            TestCase testCase,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the ephemeral public key until no longer valid
                while (true)
                {
                    serverKas.Scheme.EphemeralKeyPair.PublicQ = new EccPoint(
                        serverKas.Scheme.EphemeralKeyPair.PublicQ.X + 2,
                        serverKas.Scheme.EphemeralKeyPair.PublicQ.Y
                    );
                    if (!KeyValidationHelper.PerformEccPublicKeyValidation(
                        serverKas.Scheme.DomainParameters.CurveE,
                        serverKas.Scheme.EphemeralKeyPair.PublicQ,
                        false))
                    {
                        break;
                    }
                }
            }
        }

        private static void MangleIutStaticPrivateKey(
            TestCase testCase,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static private key to make it invalid
                iutKas.Scheme.StaticKeyPair.PrivateD += 2;
            }
        }

        private static void MangleIutStaticPublicKey(
            TestCase testCase,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                testCase.FailureTest = true;
                // modify the static public key until no longer valid
                while (true)
                {
                    iutKas.Scheme.StaticKeyPair.PublicQ = new EccPoint(
                        iutKas.Scheme.StaticKeyPair.PublicQ.X + 2,
                        iutKas.Scheme.StaticKeyPair.PublicQ.Y
                    );
                    if (!KeyValidationHelper.PerformEccPublicKeyValidation(
                        iutKas.Scheme.DomainParameters.CurveE,
                        iutKas.Scheme.StaticKeyPair.PublicQ,
                        false))
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
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            KasResult iutResult
        )
        {
            testCase.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateD ?? 0;
            testCase.StaticPublicKeyServerX = serverKas.Scheme.StaticKeyPair?.PublicQ?.X ?? 0;
            testCase.StaticPublicKeyServerY = serverKas.Scheme.StaticKeyPair?.PublicQ?.Y ?? 0;
            testCase.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateD ?? 0;
            testCase.EphemeralPublicKeyServerX = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.X ?? 0;
            testCase.EphemeralPublicKeyServerY = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.Y ?? 0;
            testCase.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            testCase.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateD ?? 0;
            testCase.StaticPublicKeyIutX = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.X ?? 0;
            testCase.StaticPublicKeyIutY = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.Y ?? 0;
            testCase.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateD ?? 0;
            testCase.EphemeralPublicKeyIutX = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.X ?? 0;
            testCase.EphemeralPublicKeyIutY = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.Y ?? 0;

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