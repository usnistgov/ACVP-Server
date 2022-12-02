using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ffc
{
    public class SchemeFfcDhHybridOneFlow : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _diffieHellman;

        public SchemeFfcDhHybridOneFlow(
            IDsaFfc dsa,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesFfc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> diffieHellman
        )
            : base(
                dsa,
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
            _diffieHellman = diffieHellman;

            if (SchemeParameters.KasAlgoAttributes.Scheme != FfcScheme.DhHybridOneFlow)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasAlgoAttributes.Scheme));
            }
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
        {
            // static secret composed of both parties static key parts
            var staticSecret = _diffieHellman.GenerateSharedSecretZ(
                DomainParameters,
                StaticKeyPair,
                otherPartyInformation.StaticPublicKey
            ).SharedSecretZ;

            // differing ephemeralSecret logic depending on key agreement role
            BitString ephemeralSecret;
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                    DomainParameters,
                    EphemeralKeyPair,
                    otherPartyInformation.StaticPublicKey
                ).SharedSecretZ;
            }
            else
            {
                ephemeralSecret = _diffieHellman.GenerateSharedSecretZ(
                    DomainParameters,
                    StaticKeyPair,
                    otherPartyInformation.EphemeralPublicKey
                ).SharedSecretZ;
            }

            // Shared secret is composed (Z_e || Z_s)
            return BitString.ConcatenateBits(ephemeralSecret, staticSecret);
        }

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            StaticKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;

            // Only party U generates an ephemeral key
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;
            }

            // When party V, KC, Bilateral, generate ephemeral nonce
            // When party V, KC, Unilateral, and the recipient of key confirmation, ephemeral nonce
            // Otherwise, no ephemeral nonce.
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV &&
                SchemeParameters.KasMode == KasMode.KdfKc)
            {
                if (SchemeParameters.KeyConfirmationDirection == KeyConfirmationDirection.Bilateral ||
                    (
                        SchemeParameters.KeyConfirmationDirection == KeyConfirmationDirection.Unilateral &&
                        SchemeParameters.KeyConfirmationRole == KeyConfirmationRole.Recipient
                    )
                )
                {
                    EphemeralNonce = EntropyProvider.GetEntropy(new BitString(DomainParameters.P).BitLength);
                }
            }

            // when party U and KdfNoKc, a NoKeyConfirmationNonce is needed.
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                && SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(128);
            }
        }
    }
}
