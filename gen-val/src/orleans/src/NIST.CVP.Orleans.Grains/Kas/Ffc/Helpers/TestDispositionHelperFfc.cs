using System;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Orleans.Grains.Kas.Ffc.Helpers
{
    public static class TestDispositionHelperFfc
    {
        #region MangleKeys
        /// <summary>
        /// Used to mangle a private or public key, based on the <see cref="KasValTestDisposition"/>
        /// </summary>
        /// <param name="result">The current result case</param>
        /// <param name="dispositionOption">The disposition to </param>
        /// <param name="serverKas"></param>
        /// <param name="iutKas"></param>
        public static void MangleKeys(
            KasValResultFfc result,
            KasValTestDisposition dispositionOption,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> iutKas
        )
        {
            var serverKeyExpectations = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                serverKas.Scheme.SchemeParameters.KasAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                serverKas.Scheme.SchemeParameters.KeyAgreementRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationRole,
                serverKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );
            var iutKeyExpectations = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                iutKas.Scheme.SchemeParameters.KasAlgoAttributes.Scheme,
                serverKas.Scheme.SchemeParameters.KasMode,
                iutKas.Scheme.SchemeParameters.KeyAgreementRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationRole,
                iutKas.Scheme.SchemeParameters.KeyConfirmationDirection
            );

            switch (dispositionOption)
            {
                case KasValTestDisposition.FailAssuranceServerStaticPublicKey:
                    MangleServerStaticPublicKey(result, serverKas, serverKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case KasValTestDisposition.FailAssuranceServerEphemeralPublicKey:
                    MangleServerEphemeralPublicKey(result, serverKas,
                        serverKeyExpectations.GeneratesEphemeralKeyPair);
                    break;
                case KasValTestDisposition.FailAssuranceIutStaticPrivateKey:
                    MangleIutStaticPrivateKey(result, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case KasValTestDisposition.FailAssuranceIutStaticPublicKey:
                    MangleIutStaticPublicKey(result, iutKas, iutKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case KasValTestDisposition.Success:
                case KasValTestDisposition.SuccessLeadingZeroNibbleZ:
                case KasValTestDisposition.SuccessLeadingZeroNibbleDkm:
                case KasValTestDisposition.FailChangedZ:
                case KasValTestDisposition.FailChangedDkm:
                case KasValTestDisposition.FailChangedOi:
                case KasValTestDisposition.FailChangedMacData:
                case KasValTestDisposition.FailChangedTag:
                    // These are not key mangling dispositions, do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dispositionOption), dispositionOption, null);
            }
        }

        private static void MangleServerStaticPublicKey(
            KasValResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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
            KasValResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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
            KasValResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
                // modify the static private key to make it invalid
                iutKas.Scheme.StaticKeyPair.PrivateKeyX += 2;
            }
        }

        private static void MangleIutStaticPublicKey(
            KasValResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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

        public static void SetResultInformationFromKasProcessing(
            KasValParametersFfc param,
            KasValResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>,
                FfcDomainParameters, FfcKeyPair> iutKas,
            KasResult iutResult
        )
        {
            result.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            result.StaticPublicKeyServer = serverKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;
            result.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            result.EphemeralPublicKeyServer = serverKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;
            result.DkmNonceServer = serverKas.Scheme.DkmNonce;
            result.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            result.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateKeyX ?? 0;
            result.StaticPublicKeyIut = iutKas?.Scheme?.StaticKeyPair?.PublicKeyY ?? 0;
            result.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateKeyX ?? 0;
            result.EphemeralPublicKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PublicKeyY ?? 0;
            result.DkmNonceIut = iutKas?.Scheme?.DkmNonce;
            result.EphemeralNonceIut = iutKas?.Scheme?.EphemeralNonce;

            result.Z = iutResult?.Z;
            result.OtherInfo = iutResult?.Oi;
            result.OiLen = result.OtherInfo?.BitLength ?? 0;
            result.Dkm = iutResult?.Dkm;
            result.MacData = iutResult?.MacData;
            if (param.KasMode == KasMode.NoKdfNoKc)
            {
                result.HashZ = iutResult?.Tag;
            }
            else
            {
                result.Tag = iutResult?.Tag;
            }
        }

        public static void SetResultInformationFromKasProcessing(
            KasAftParametersFfc param,
            KasAftResultFfc result,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> iutKas,
            KasResult iutResult
        )
        {
            result.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateKeyX ?? 0;
            result.StaticPublicKeyServer = serverKas.Scheme.StaticKeyPair?.PublicKeyY ?? 0;
            result.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateKeyX ?? 0;
            result.EphemeralPublicKeyServer = serverKas.Scheme.EphemeralKeyPair?.PublicKeyY ?? 0;
            result.DkmNonceServer = serverKas.Scheme.DkmNonce;
            result.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            result.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateKeyX ?? 0;
            result.StaticPublicKeyIut = iutKas?.Scheme?.StaticKeyPair?.PublicKeyY ?? 0;
            result.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateKeyX ?? 0;
            result.EphemeralPublicKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PublicKeyY ?? 0;
            result.DkmNonceIut = iutKas?.Scheme?.DkmNonce;
            result.EphemeralNonceIut = iutKas?.Scheme?.EphemeralNonce;

            result.Z = iutResult?.Z;
            result.OtherInfo = iutResult?.Oi;
            result.OiLen = result.OtherInfo?.BitLength ?? 0;
            result.Dkm = iutResult?.Dkm;
            result.MacData = iutResult?.MacData;
            if (param.KasMode == KasMode.NoKdfNoKc)
            {
                result.HashZ = iutResult?.Tag;
            }
            else
            {
                result.Tag = iutResult?.Tag;
            }
        }
    }
}
