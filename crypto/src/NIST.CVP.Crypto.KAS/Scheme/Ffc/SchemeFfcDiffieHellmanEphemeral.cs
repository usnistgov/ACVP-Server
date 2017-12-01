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
    public class SchemeFfcDiffieHellmanEphemeral : SchemeBaseFfc
    {
        private readonly IDiffieHellman<FfcDomainParameters, FfcKeyPair> Dh;

        public SchemeFfcDiffieHellmanEphemeral(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<KasDsaAlgoAttributesFfc> schemeParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            IDiffieHellman<FfcDomainParameters, FfcKeyPair> dh
        ) 
            : base(dsa, kdfFactory, keyConfirmationFactory, noKeyConfirmationFactory, otherInfoFactory, entropyProvider, schemeParameters, kdfParameters, macParameters)
        {
            Dh = dh;

            if (SchemeParameters.KasDsaAlgoAttributes.Scheme != FfcScheme.DhEphem)
            {
                throw new ArgumentException(nameof(SchemeParameters.KasDsaAlgoAttributes.Scheme));
            }

            if (SchemeParameters.KasMode == KasMode.KdfKc)
            {
                throw new ArgumentException($"{SchemeParameters.KasMode} not possible with {SchemeParameters.KasDsaAlgoAttributes.Scheme}");
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Generate the domain parameters if null, 
        /// set generate a ephemeral key pair, 
        /// generate a no key confirmation nonce when the u/initiator party
        /// </summary>
        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters).KeyPair;

            // when party U and KdfNoKc, a NoKeyConfirmationNonce is needed.
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU 
                && SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(128);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Generates the shared secret.  
        /// Shared secret Z is made up of this party's private key along with the other parties public key, 
        /// run through the DiffieHellman primitive.
        /// </summary>
        /// <param name="otherPartyInformation"></param>
        /// <returns></returns>
        protected override BitString ComputeSharedSecret(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
        {
            return Dh.GenerateSharedSecretZ(
                DomainParameters, 
                EphemeralKeyPair,
                otherPartyInformation.EphemeralPublicKey
            ).SharedSecretZ;
        }
    }
}
