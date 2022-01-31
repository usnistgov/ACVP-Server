using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc
{
    public abstract class SchemeBaseEcc
        : SchemeBase<
            SchemeParametersBase<KasDsaAlgoAttributesEcc>,
            KasDsaAlgoAttributesEcc,
            OtherPartySharedInformation<
                EccDomainParameters,
                EccKeyPair
            >,
            EccDomainParameters,
            EccKeyPair
        >
    {
        protected readonly IDsaEcc Dsa;
        protected readonly IEccCurveFactory EccCurveFactory;

        protected SchemeBaseEcc(
            IDsaEcc dsa,
            IEccCurveFactory eccCurveFactory,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesEcc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters
        )
            : base(
                  kdfFactory,
                  keyConfirmationFactory,
                  noKeyConfirmationFactory,
                  otherInfoFactory,
                  entropyProvider,
                  schemeParameters,
                  kdfParameters,
                  macParameters
              )
        {
            Dsa = dsa;
            EccCurveFactory = eccCurveFactory;
        }

        /// <inheritdoc />
        public override int OtherInputLength => 376;

        public override OtherPartySharedInformation<EccDomainParameters, EccKeyPair> ReturnPublicInfoThisParty()
        {
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            return new OtherPartySharedInformation<EccDomainParameters, EccKeyPair>(
                DomainParameters,
                SchemeParameters.ThisPartyId,
                StaticKeyPair,
                EphemeralKeyPair,
                DkmNonce,
                EphemeralNonce,
                NoKeyConfirmationNonce
            );
        }

        protected override ISha GetShaInstanceFromDsa()
        {
            return Dsa.Sha;
        }

        /// <inheritdoc />
        protected override bool KeyValidityChecks(OtherPartySharedInformation<EccDomainParameters, EccKeyPair> otherPartyInformation)
        {
            // When KeyPairGen or FullVal, check the static public key
            if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.KeyPairGen) ||
                SchemeParameters.KasAssurances.HasFlag(KasAssurance.PartialVal) &&
                StaticKeyPair != null)
            {
                if (!Dsa.ValidateKeyPair(DomainParameters, StaticKeyPair).Success)
                {
                    return false;
                }
            }

            // When fullval, and the other party provides a static public key
            if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.PartialVal) &&
                otherPartyInformation.StaticPublicKey != null)
            {
                if (!Dsa.ValidateKeyPair(DomainParameters, otherPartyInformation.StaticPublicKey).Success)
                {
                    return false;
                }
            }

            // When fullval, and the other party provides a ephemeral public key
            if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.PartialVal) &&
                otherPartyInformation.EphemeralPublicKey != null)
            {
                if (!Dsa.ValidateKeyPair(DomainParameters, otherPartyInformation.EphemeralPublicKey).Success)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        protected override void GenerateDomainParameters()
        {
            DomainParameters = new EccDomainParameters(
                EccCurveFactory.GetCurve(
                    SchemeParameters.KasAlgoAttributes.CurveName
                )
            );
        }

        /// <inheritdoc />
        protected override BitString GetEphemeralKeyOrNonce(EccKeyPair ephemeralPublicKey, BitString ephemeralNonce, BitString dkmNonce)
        {
            if (ephemeralPublicKey?.PublicQ != null && ephemeralPublicKey.PublicQ?.X != 0)
            {
                var exactLength = CurveAttributesHelper.GetCurveAttribute(DomainParameters.CurveE.CurveName).DegreeOfPolynomial;

                return BitString.ConcatenateBits(
                    SharedSecretZHelper.FormatEccSharedSecretZ(ephemeralPublicKey.PublicQ.X, exactLength),
                    SharedSecretZHelper.FormatEccSharedSecretZ(ephemeralPublicKey.PublicQ.Y, exactLength)
                );
            }

            if (ephemeralNonce != null && ephemeralNonce?.BitLength != 0)
            {
                return ephemeralNonce;
            }

            return dkmNonce;
        }
    }
}
