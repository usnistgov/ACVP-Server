using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBase : IScheme
    {
        protected IDsaFfc Dsa;
        protected IKdfFactory KdfFactory;
        protected IKeyConfirmationFactory KeyConfirmationFactory;
        protected INoKeyConfirmationFactory NoKeyConfirmationFactory;
        protected IOtherInfoFactory OtherInfoFactory;
        protected IEntropyProvider EntropyProvider;
        protected KasParameters KasParameters;
        protected KdfParameters KdfParameters;
        protected MacParameters MacParameters;

        protected SchemeBase(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            KasParameters kasParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters
        )
        {
            Dsa = dsa;
            KdfFactory = kdfFactory;
            KeyConfirmationFactory = keyConfirmationFactory;
            NoKeyConfirmationFactory = noKeyConfirmationFactory;
            OtherInfoFactory = otherInfoFactory;
            EntropyProvider = entropyProvider;
            KasParameters = kasParameters;
            KdfParameters = kdfParameters;
            MacParameters = macParameters;
        }

        /// <inheritdoc />
        public int OtherInputLength => 240;
        /// <inheritdoc />
        public abstract FfcScheme Scheme { get; }
        /// <inheritdoc />
        public FfcDomainParameters DomainParameters { get; protected set; }
        /// <inheritdoc />
        public FfcKeyPair StaticKeyPair { get; protected set; }
        /// <inheritdoc />
        public FfcKeyPair EphemeralKeyPair { get; protected set; }
        /// <inheritdoc />
        public BitString EphemeralNonce { get; protected set; }
        /// <inheritdoc />
        public BitString DkmNonce { get; protected set; }
        /// <inheritdoc />
        public BitString NoKeyConfirmationNonce { get; protected set; }

        /// <summary>
        /// flag is used to determine if this party's private/public key/nonce information has already been generated.
        /// </summary>
        protected bool ThisPartyKeysGenerated => (
            StaticKeyPair != null || 
            EphemeralKeyPair != null || 
            EphemeralNonce != null ||
            DkmNonce != null ||
            NoKeyConfirmationNonce != null
        );

        /// <inheritdoc />
        public void SetDomainParameters(FfcDomainParameters domainParameters)
        {
            DomainParameters = domainParameters;
        }

        /// <inheritdoc />
        public FfcSharedInformation ReturnPublicInfoThisParty()
        {
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            return new FfcSharedInformation(
                DomainParameters,
                KasParameters.ThisPartyId,
                StaticKeyPair.PublicKeyY,
                EphemeralKeyPair.PublicKeyY,
                EphemeralNonce,
                DkmNonce,
                NoKeyConfirmationNonce
            );
        }

        /// <inheritdoc />
        public KasResult ComputeResult(FfcSharedInformation otherPartyInformation)
        {
            // Set this instance's domain parameters equal to the other party's assuming they were passed in
            if (otherPartyInformation.DomainParameters != null)
            {
                DomainParameters = otherPartyInformation.DomainParameters;
            }

            // this party's key/nonce information has not yet been generated, generate it.
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
            var computedKeyMac = ComputeKeyMac(otherPartyInformation, dkm.DerivedKey);

            return new KasResult(z, oi, dkm.DerivedKey, computedKeyMac.MacData, computedKeyMac.Mac);
        }

        /// <summary>
        /// Generate a set of domain parameters
        /// </summary>
        protected void GenerateDomainParameters()
        {
            var paramDetails = FfcParameterSetDetails.GetDetailsForParameterSet(KasParameters.FfcParameterSet);

            // TODO validate generation mode correct
            SetDomainParameters(Dsa.GenerateDomainParameters(new FfcDomainParametersGenerateRequest(
                paramDetails.qLength,
                paramDetails.pLength,
                paramDetails.qLength,
                Dsa.Sha.HashFunction.OutputLen,
                PrimeGenMode.Provable,
                GeneratorGenMode.Canonical
            )).PqgDomainParameters);
        }

        protected INoKeyConfirmationParameters GetNoKeyConfirmationParameters(BitString derivedKeyingMaterial)
        {
            if (MacParameters.MacType == KeyAgreementMacType.AesCcm)
            {
                return new NoKeyConfirmationParameters(
                    MacParameters.MacType,
                    MacParameters.MacLength,
                    derivedKeyingMaterial,
                    NoKeyConfirmationNonce,
                    MacParameters.CcmNonce
                );
            }

            return new NoKeyConfirmationParameters(
                MacParameters.MacType,
                MacParameters.MacLength,
                derivedKeyingMaterial,
                NoKeyConfirmationNonce
            );
        }

        /// <summary>
        /// Generates key pairs and nonce information specific to the scheme selected
        /// </summary>
        protected abstract void GenerateKasKeyNonceInformation();

        /// <summary>
        /// Computes a shared secret based off of party U and party V inputs based on the scheme
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <returns></returns>
        protected abstract BitString ComputeSharedSecret(FfcSharedInformation otherPartyInformation);

        /// <summary>
        /// Generate the OtherInformation that is to be plugged into a KDF function.
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <returns></returns>
        protected abstract IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation);

        /// <summary>
        /// Compute's the MAC of a key for both key confirmation and no key confirmation
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <param name="derivedKeyingMaterial">The derived keying material generated via KDF</param>
        /// <returns></returns>
        protected abstract ComputeKeyMacResult ComputeKeyMac(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial);
    }
}