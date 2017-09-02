using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBase : IScheme
    {
        private const int OTHER_INFO_LENGTH = 240;

        protected IDsaFfc Dsa;
        protected IKdfFactory KdfFactory;
        protected IKeyConfirmationFactory KeyConfirmationFactory;
        protected IOtherInfoFactory OtherInfoFactory;
        protected KasParameters KasParameters;
        protected KdfParameters KdfParameters;
        protected MacParameters MacParameters;

        protected SchemeBase(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            IOtherInfoFactory otherInfoFactory,
            KasParameters kasParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters
        )
        {
            Dsa = dsa;
            KdfFactory = kdfFactory;
            KeyConfirmationFactory = keyConfirmationFactory;
            OtherInfoFactory = otherInfoFactory;
            KasParameters = kasParameters;
            KdfParameters = kdfParameters;
            MacParameters = macParameters;

            
        }

        public abstract FfcScheme FfcScheme { get; }

        public FfcScheme Scheme { get; }
        public FfcDomainParameters DomainParameters { get; }
        public FfcKeyPair StaticKeyPair { get; protected set; }
        public FfcKeyPair EphemeralKeyPair { get; protected set; }
        public BitString EphemeralNonce { get; protected set; }
        public BitString DkmNonce { get; protected set; }
        protected bool ThisPartyKeysGenerated => (
            StaticKeyPair != null || 
            EphemeralKeyPair != null || 
            EphemeralNonce != null ||
            DkmNonce != null
        );

        public FfcSharedInformation ReturnPublicInfoForOtherParty()
        {
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            return new FfcSharedInformation(
                StaticKeyPair.PublicKeyY,
                EphemeralKeyPair.PublicKeyY,
                EphemeralNonce,
                DkmNonce
            );
        }

        public KasResult ComputeResult(FfcSharedInformation otherPartyInformation)
        {
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            // Get shared secret, differs dependant on scheme
            var z = ComputeSharedSecret(otherPartyInformation);

            // Component only test
            if (MacParameters == null)
            {
                // The SHA used is the same used in the DSA instance
                var tag = Dsa.Sha.HashMessage(z);

                return new KasResult(z, tag.Digest);
            }

            // Build OI, differs dependant on scheme
            var oi = GenerateOtherInformation(otherPartyInformation).GetOtherInfo();

            // Get keying material
            var kdf = KdfFactory.GetInstance(KdfHashMode.Sha, Dsa.Sha.HashFunction);
            var dkm = kdf.DeriveKey(z, KdfParameters.KeyLength, oi);

            // Perform key confirmation, differs depending on scheme
            var computedKeyMac = ComputeKeyMac(otherPartyInformation);

            return new KasResult(z, oi, dkm.DerivedKey, computedKeyMac.MacData, computedKeyMac.Mac);
        }

        protected abstract void GenerateKasKeyNonceInformation();

        protected abstract IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation);

        protected abstract BitString ComputeSharedSecret(FfcSharedInformation otherPartyInformation);

        protected abstract ComputeKeyMacResult ComputeKeyMac(FfcSharedInformation otherPartyInformation);
    }
}