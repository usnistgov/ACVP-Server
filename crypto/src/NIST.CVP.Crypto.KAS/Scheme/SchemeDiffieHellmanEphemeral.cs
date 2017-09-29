using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeDiffieHellmanEphemeral : SchemeBase
    {
        private readonly IDiffieHellman Dh;

        public SchemeDiffieHellmanEphemeral(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParameters schemeParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            IDiffieHellman dh
        ) 
            : base(dsa, kdfFactory, keyConfirmationFactory, noKeyConfirmationFactory, otherInfoFactory, entropyProvider, schemeParameters, kdfParameters, macParameters)
        {
            Dh = dh;

            if (SchemeParameters.Scheme != FfcScheme.DhEphem)
            {
                throw new ArgumentException(nameof(SchemeParameters.Scheme));
            }

            if (SchemeParameters.KasMode == KasMode.KdfKc)
            {
                throw new ArgumentException($"{SchemeParameters.KasMode} not possible with {SchemeParameters.Scheme}");
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

            // TODO confirm party u always generates in specification
            // Key confirmation not possible for dhEphem scheme, MACData requires the use of a nonce
            // Initiator should generate (doesn't actually matter who generates, just that someone does)
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.Initiator 
                && SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(128);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Generates the shared secret.  
        /// Shared secret Z is made up of this party's private key along with the other parties public key, 
        /// run through the <see cref="IDiffieHellman{TDsaDomainParameters} "/> primitive.
        /// </summary>
        /// <param name="otherPartyInformation"></param>
        /// <returns></returns>
        protected override BitString ComputeSharedSecret(FfcSharedInformation otherPartyInformation)
        {
            return Dh.GenerateSharedSecretZ(DomainParameters.P, EphemeralKeyPair.PrivateKeyX,
                otherPartyInformation.EphemeralPublicKey).SharedSecretZ;
        }

        /// <inheritdoc />
        protected override IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation)
        {
            return OtherInfoFactory.GetInstance(
                KdfParameters.OtherInfoPattern, 
                OtherInputLength,
                SchemeParameters.KeyAgreementRole, 
                ReturnPublicInfoThisParty(), 
                otherPartyInformation
            );
        }

        /// <inheritdoc />
        protected override ComputeKeyMacResult ComputeKeyMac(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial)
        {
            // key confirmation not possible with this scheme, proceed with no key confirmation
            // No key confirmation nonce provided by party u.
            var noKeyConfirmationNonce = SchemeParameters.KeyAgreementRole == KeyAgreementRole.Initiator
                ? NoKeyConfirmationNonce
                : otherPartyInformation.NoKeyConfirmationNonce;

            var noKeyConfirmationParameters = GetNoKeyConfirmationParameters(derivedKeyingMaterial, noKeyConfirmationNonce);

            var noKeyConfirmationInstance = NoKeyConfirmationFactory.GetInstance(noKeyConfirmationParameters);
            
            return noKeyConfirmationInstance.ComputeMac();
        }
    }
}
