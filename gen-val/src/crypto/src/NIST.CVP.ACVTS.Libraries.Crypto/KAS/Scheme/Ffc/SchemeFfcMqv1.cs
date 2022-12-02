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
    public class SchemeFfcMqv1 : SchemeBaseFfc
    {
        private readonly IMqv<FfcDomainParameters, FfcKeyPair> _mqv;

        public SchemeFfcMqv1(
            IDsaFfc dsa,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesFfc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            IMqv<FfcDomainParameters, FfcKeyPair> mqv
        )
            : base(dsa, kdfFactory, keyConfirmationFactory, noKeyConfirmationFactory, otherInfoFactory, entropyProvider, schemeParameters, kdfParameters, macParameters)
        {
            _mqv = mqv;

            if (SchemeParameters.KasAlgoAttributes.Scheme != FfcScheme.Mqv1)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasAlgoAttributes.Scheme));
            }
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

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
        {
            // Party U uses both its static and ephemeral keys, and Party V's static public key
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return _mqv.GenerateSharedSecretZ(
                    DomainParameters,
                    StaticKeyPair,
                    otherPartyInformation.StaticPublicKey,
                    EphemeralKeyPair,
                    EphemeralKeyPair,
                    otherPartyInformation.StaticPublicKey
                ).SharedSecretZ;
            }

            // Party V uses its static key, and party U's static and ephemeral keys
            return _mqv.GenerateSharedSecretZ(
                DomainParameters,
                StaticKeyPair,
                otherPartyInformation.StaticPublicKey,
                StaticKeyPair,
                StaticKeyPair,
                otherPartyInformation.EphemeralPublicKey
            ).SharedSecretZ;
        }
    }
}
