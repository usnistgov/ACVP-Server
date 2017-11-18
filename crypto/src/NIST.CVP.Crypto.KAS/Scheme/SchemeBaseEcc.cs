using System;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBaseEcc 
        : SchemeBase<
            SchemeParametersBase<
                EccParameterSet, 
                EccScheme
            >, 
            EccParameterSet, 
            EccScheme, 
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
            IKdfFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<EccParameterSet, EccScheme> schemeParameters,
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
                EphemeralNonce,
                DkmNonce,
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
            // TODO NEED TO PASS CORRECT CURVE
            DomainParameters = new EccDomainParameters(EccCurveFactory.GetCurve(Curve.B163));
        }

        /// <inheritdoc />
        protected override BitString GetEphemeralKeyOrNonce(EccKeyPair ephemeralPublicKey, BitString ephemeralNonce)
        {
            if (ephemeralPublicKey != null && ephemeralPublicKey?.PublicQ.X != 0)
            {
                var exactLength = DomainParameters.CurveE.OrderN.ExactBitLength();
                return SharedSecretZHelper.FormatEccSharedSecretZ(ephemeralPublicKey.PublicQ, exactLength);
            }

            return ephemeralNonce;
        }
    }
}