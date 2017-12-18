using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme.Ffc
{
    public class SchemeFfcDhStatic : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> _diffieHellman;

        public SchemeFfcDhStatic(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
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

            if (SchemeParameters.KasDsaAlgoAttributes.Scheme != FfcScheme.DhStatic)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasDsaAlgoAttributes.Scheme));
            }
        }

        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
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

            // DKM Nonce required when party U and KdfNoKc or KdfKc
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU &&
                SchemeParameters.KasMode != KasMode.NoKdfNoKc)
            {
                try
                {
                    DkmNonce = EntropyProvider.GetEntropy(new BitString(DomainParameters.Q).BitLength / 2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
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