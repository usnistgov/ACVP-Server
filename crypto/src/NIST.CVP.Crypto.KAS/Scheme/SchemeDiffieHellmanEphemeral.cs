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
        private readonly IDiffieHellman<FfcDomainParameters> Dh;

        public SchemeDiffieHellmanEphemeral(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            KasParameters kasParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            IDiffieHellman<FfcDomainParameters> dh
        ) 
            : base(dsa, kdfFactory, keyConfirmationFactory, noKeyConfirmationFactory, otherInfoFactory, entropyProvider, kasParameters, kdfParameters, macParameters)
        {
            Dh = dh;
        }

        public override FfcScheme Scheme => FfcScheme.DhEphem;

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

            // Key confirmation not possible for dhEphem scheme, MACData requires the use of a nonce
            // Initiator should generate (doesn't actually matter who generates, just that someone does)
            if (KasParameters.KeyAgreementRole == KeyAgreementRole.UPartyInitiator)
            {
                // TODO don't remove zeros?
                BitString q = new BitString(DomainParameters.Q, 0, true);
                NoKeyConfirmationNonce = EntropyProvider.GetEntropy(q.BitLength / 2);
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
            return Dh.GenerateSharedSecretZ(DomainParameters, EphemeralKeyPair.PrivateKeyX,
                otherPartyInformation.EphemeralPublicKey).SharedSecretZ;
        }

        protected override IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation)
        {
            return OtherInfoFactory.GetInstance(
                KdfParameters.OtherInfoPattern, 
                OtherInputLength,
                KasParameters.KeyAgreementRole, 
                ReturnPublicInfoThisParty(), 
                otherPartyInformation
            );
        }

        protected override ComputeKeyMacResult ComputeKeyMac(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial)
        {
            // key confirmation not possible with this scheme, proceed with no key confirmation
            var noKeyConfirmationParameters = GetNoKeyConfirmationParameters(derivedKeyingMaterial);

            var noKeyConfirmationInstance = NoKeyConfirmationFactory.GetInstance(noKeyConfirmationParameters);
            
            return noKeyConfirmationInstance.ComputeMac();
        }
    }
}
