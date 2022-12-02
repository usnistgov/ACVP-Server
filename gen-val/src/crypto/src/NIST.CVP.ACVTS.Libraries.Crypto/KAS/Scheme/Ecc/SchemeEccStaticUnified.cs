using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
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
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc
{
    public class SchemeEccStaticUnified : SchemeBaseEcc
    {
        private readonly IDiffieHellman<EccDomainParameters, EccKeyPair> _diffieHellman;

        public SchemeEccStaticUnified(
            IDsaEcc dsa,
            IEccCurveFactory eccCurveFactory,
            IKdfOneStepFactory kdfFactory,
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesEcc> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters,
            IDiffieHellman<EccDomainParameters, EccKeyPair> diffieHellman
        )
            : base(
                dsa,
                eccCurveFactory,
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
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<EccDomainParameters, EccKeyPair> otherPartyInformation)
        {
            return _diffieHellman.GenerateSharedSecretZ(
                DomainParameters,
                StaticKeyPair,
                otherPartyInformation.StaticPublicKey
            ).SharedSecretZ;
        }

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            StaticKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;

            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(DomainParameters.CurveE.CurveName);

            // DKM Nonce required when party U and KdfNoKc/KdfKc
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU &&
                SchemeParameters.KasMode != KasMode.NoKdfNoKc)
            {
                DkmNonce = EntropyProvider.GetEntropy(curveAttributes.DegreeOfPolynomial.ValueToMod(BitString.BITSINBYTE));
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
                    EphemeralNonce = EntropyProvider.GetEntropy(curveAttributes.DegreeOfPolynomial.ValueToMod(BitString.BITSINBYTE));
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
