using System;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Orleans.Grains.Kas.Ecc.Helpers
{
    public static class TestDispositionHelperEcc
    {
        #region MangleKeys
        /// <summary>
        /// Used to mangle a private or public key, based on the <see cref="dispositionOption"/>
        /// </summary>
        /// <param name="result">The current result</param>
        /// <param name="dispositionOption">The disposition to acheive</param>
        /// <param name="serverKas">The server instance of kas</param>
        /// <param name="iutKas">The iut instance of kas</param>
        public static void MangleKeys(
            KasValResultEcc result,
            KasValTestDisposition dispositionOption,
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
                case KasValTestDisposition.FailAssuranceServerStaticPublicKey:
                    MangleServerStaticPublicKey(result, serverKas, serverKeyExpectations.GeneratesStaticKeyPair);
                    break;
                case KasValTestDisposition.FailAssuranceServerEphemeralPublicKey:
                    MangleServerEphemeralPublicKey(result, serverKas, serverKeyExpectations.GeneratesEphemeralKeyPair);
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
            KasValResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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
            KasValResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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
            KasValResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
                // modify the static private key to make it invalid
                iutKas.Scheme.StaticKeyPair.PrivateD += 2;
            }
        }

        private static void MangleIutStaticPublicKey(
            KasValResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            bool generatesKeyPair
        )
        {
            if (generatesKeyPair)
            {
                result.TestPassed = false;
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

        public static void SetResultInformationFromKasProcessing(
            KasValParametersEcc param,
            KasValResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            KasResult iutResult
        )
        {
            result.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateD ?? 0;
            result.StaticPublicKeyServerX = serverKas.Scheme.StaticKeyPair?.PublicQ?.X ?? 0;
            result.StaticPublicKeyServerY = serverKas.Scheme.StaticKeyPair?.PublicQ?.Y ?? 0;
            result.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateD ?? 0;
            result.EphemeralPublicKeyServerX = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.X ?? 0;
            result.EphemeralPublicKeyServerY = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.Y ?? 0;
            result.DkmNonceServer = serverKas.Scheme.DkmNonce;
            result.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            result.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateD ?? 0;
            result.StaticPublicKeyIutX = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.X ?? 0;
            result.StaticPublicKeyIutY = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.Y ?? 0;
            result.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateD ?? 0;
            result.EphemeralPublicKeyIutX = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.X ?? 0;
            result.EphemeralPublicKeyIutY = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.Y ?? 0;
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
            KasAftParametersEcc param,
            KasAftResultEcc result,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> serverKas,
            IKas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> iutKas,
            KasResult iutResult
        )
        {
            result.StaticPrivateKeyServer = serverKas.Scheme.StaticKeyPair?.PrivateD ?? 0;
            result.StaticPublicKeyServerX = serverKas.Scheme.StaticKeyPair?.PublicQ?.X ?? 0;
            result.StaticPublicKeyServerY = serverKas.Scheme.StaticKeyPair?.PublicQ?.Y ?? 0;
            result.EphemeralPrivateKeyServer = serverKas.Scheme.EphemeralKeyPair?.PrivateD ?? 0;
            result.EphemeralPublicKeyServerX = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.X ?? 0;
            result.EphemeralPublicKeyServerY = serverKas.Scheme.EphemeralKeyPair?.PublicQ?.Y ?? 0;
            result.DkmNonceServer = serverKas.Scheme.DkmNonce;
            result.EphemeralNonceServer = serverKas.Scheme.EphemeralNonce;

            result.StaticPrivateKeyIut = iutKas?.Scheme?.StaticKeyPair?.PrivateD ?? 0;
            result.StaticPublicKeyIutX = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.X ?? 0;
            result.StaticPublicKeyIutY = iutKas?.Scheme?.StaticKeyPair?.PublicQ?.Y ?? 0;
            result.EphemeralPrivateKeyIut = iutKas?.Scheme?.EphemeralKeyPair?.PrivateD ?? 0;
            result.EphemeralPublicKeyIutX = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.X ?? 0;
            result.EphemeralPublicKeyIutY = iutKas?.Scheme?.EphemeralKeyPair?.PublicQ?.Y ?? 0;
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
