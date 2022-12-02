using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
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

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc
{
    public class SchemeEccOnePassDh : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccOnePassDh(
            IDsaEcc dsa,
            IEccCurveFactory curveFactory,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesEcc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman
        ) :
            base(
                dsa,
                curveFactory,
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

            if (SchemeParameters.KasAlgoAttributes.Scheme != EccScheme.OnePassDh)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasAlgoAttributes.Scheme));
            }

            if (SchemeParameters.KeyConfirmationDirection == KeyConfirmationDirection.Bilateral ||
                (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU &&
                 SchemeParameters.KeyConfirmationRole == KeyConfirmationRole.Provider) ||
                (SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV &&
                SchemeParameters.KeyConfirmationRole == KeyConfirmationRole.Recipient))
            {
                throw new ArgumentException("Only unilateral Key confirmation possible party V to U");
            }
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<EccDomainParameters, EccKeyPair> otherPartyInformation)
        {
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return _diffieHellman.GenerateSharedSecretZ(
                    DomainParameters,
                    EphemeralKeyPair,
                    otherPartyInformation.StaticPublicKey
                ).SharedSecretZ;
            }

            return _diffieHellman.GenerateSharedSecretZ(
                DomainParameters,
                StaticKeyPair,
                otherPartyInformation.EphemeralPublicKey
            ).SharedSecretZ;
        }

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            // Party U generates an Ephemeral Key, party V provides a static key for this scheme
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;
            }
            else
            {
                StaticKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;
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
