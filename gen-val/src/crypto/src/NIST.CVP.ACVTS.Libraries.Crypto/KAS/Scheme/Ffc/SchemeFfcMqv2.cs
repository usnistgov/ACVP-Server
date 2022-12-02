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
    public class SchemeFfcMqv2 : SchemeBaseFfc
    {
        private readonly IMqv<FfcDomainParameters, FfcKeyPair> _mqv;

        public SchemeFfcMqv2(
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
        ) :
            base(
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
            _mqv = mqv;

            if (SchemeParameters.KasAlgoAttributes.Scheme != FfcScheme.Mqv2)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasAlgoAttributes.Scheme));
            }
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
        {
            return _mqv.GenerateSharedSecretZ(
                DomainParameters,
                StaticKeyPair,
                otherPartyInformation.StaticPublicKey,
                EphemeralKeyPair,
                EphemeralKeyPair,
                otherPartyInformation.EphemeralPublicKey
            ).SharedSecretZ;
        }

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            // Both parties U and V generate static and ephemeral key pairs in this scheme
            StaticKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;
            EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;

            // when party U and KdfNoKc, a NoKeyConfirmationNonce is needed.
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                && SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(128);
            }
        }
    }
}
