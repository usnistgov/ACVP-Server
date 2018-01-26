using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBase<TSchemeParameters, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair> 
        : IScheme<TSchemeParameters, TKasDsaAlgoAttributes, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TSchemeParameters : ISchemeParameters<TKasDsaAlgoAttributes>
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {

        protected IKdfFactory KdfFactory;
        protected IKeyConfirmationFactory KeyConfirmationFactory;
        protected INoKeyConfirmationFactory NoKeyConfirmationFactory;
        protected IOtherInfoFactory OtherInfoFactory;
        protected IEntropyProvider EntropyProvider;
        protected KdfParameters KdfParameters;
        protected MacParameters MacParameters;

        protected SchemeBase(
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            TSchemeParameters schemeParameters,
            KdfParameters kdfParameters, 
            MacParameters macParameters
        )
        {
            KdfFactory = kdfFactory;
            KeyConfirmationFactory = keyConfirmationFactory;
            NoKeyConfirmationFactory = noKeyConfirmationFactory;
            OtherInfoFactory = otherInfoFactory;
            EntropyProvider = entropyProvider;
            SchemeParameters = schemeParameters;
            KdfParameters = kdfParameters;
            MacParameters = macParameters;
        }

        public abstract int OtherInputLength { get; }
        public TSchemeParameters SchemeParameters { get; protected set; }
        public TDomainParameters DomainParameters { get; protected set; }
        public TKeyPair StaticKeyPair { get; protected set; }
        public TKeyPair EphemeralKeyPair { get; protected set; }
        public BitString EphemeralNonce { get; protected set; }
        public BitString DkmNonce { get; protected set; }
        public BitString NoKeyConfirmationNonce { get; protected set; }

        /// <summary>
        /// flag is used to determine if this party's private/public key/nonce information has already been generated.
        /// </summary>
        protected bool ThisPartyKeysGenerated => (
            StaticKeyPair != null ||
            EphemeralKeyPair != null ||
            DkmNonce != null ||
            EphemeralNonce != null ||
            NoKeyConfirmationNonce != null
        );

        #region interface implementation methods
        /// <summary>
        /// Sets the domain parameters to be used in the key aggreement
        /// </summary>
        /// <param name="domainParameters"></param>
        public void SetDomainParameters(TDomainParameters domainParameters)
        {
            DomainParameters = domainParameters;
        }

        /// <summary>
        /// Returns the public information from this party, 
        /// intended for the other party for completion of the key agreement.
        /// </summary>
        /// <returns></returns>
        public abstract TOtherPartySharedInfo ReturnPublicInfoThisParty();
        
        /// <summary>
        /// Performs the key agreement
        /// </summary>
        /// <param name="otherPartyInformation">The other parties public information needed by this party for key agreement</param>
        /// <returns></returns>
        public KasResult ComputeResult(TOtherPartySharedInfo otherPartyInformation)
        {
            var sha = GetShaInstanceFromDsa();

            // Set this instance's domain parameters equal to the other party's assuming they were passed in
            if (otherPartyInformation.DomainParameters != null)
            {
                DomainParameters = otherPartyInformation.DomainParameters;
            }

            // this party's key/nonce information has not yet been generated, fail.
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            // Perform keychecks
            if (!KeyValidityChecks(otherPartyInformation))
            {
                return new KasResult("Failed key validation.");
            }

            if (otherPartyInformation.NoKeyConfirmationNonce != null)
            {
                NoKeyConfirmationNonce = otherPartyInformation.NoKeyConfirmationNonce;
            }

            // Get shared secret, differs dependant on scheme
            var z = ComputeSharedSecret(otherPartyInformation);

            // Component only test
            if (SchemeParameters.KasMode == KasMode.NoKdfNoKc)
            {
                // The SHA used is the same used in the DSA instance
                var tag = sha.HashMessage(z);

                return new KasResult(z, tag.Digest);
            }

            // Get other information
            var oi = GenerateOtherInformation(otherPartyInformation).GetOtherInfo();

            // Get keying material
            var kdf = KdfFactory.GetInstance(KdfHashMode.Sha, sha.HashFunction);
            var dkm = kdf.DeriveKey(z, KdfParameters.KeyLength, oi);

            // Perform no/key confirmation
            var computedKeyMac = ComputeMac(otherPartyInformation, dkm.DerivedKey);

            return new KasResult(z, oi, dkm.DerivedKey, computedKeyMac.MacData, computedKeyMac.Mac);
        }
        #endregion interface implementation methods

        #region protected implemented methods
        /// <summary>
        /// Generate the OtherInformation that is to be plugged into a KDF function.
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <returns></returns>
        protected IOtherInfo GenerateOtherInformation(TOtherPartySharedInfo otherPartyInformation)
        {
            var thisPartyPublicInfo = ReturnPublicInfoThisParty();
            var thisPartyOtherInfo = new PartyOtherInfo(thisPartyPublicInfo.PartyId, thisPartyPublicInfo.DkmNonce);

            var otherPartyOtherInfo = new PartyOtherInfo(otherPartyInformation.PartyId, otherPartyInformation.DkmNonce);

            return OtherInfoFactory.GetInstance(
                KdfParameters.OtherInfoPattern,
                OtherInputLength,
                SchemeParameters.KeyAgreementRole,
                thisPartyOtherInfo,
                otherPartyOtherInfo
            );
        }

        /// <summary>
        /// Compute's the MAC of a key for both key confirmation and no key confirmation
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <param name="derivedKeyingMaterial">The derived keying material generated via KDF</param>
        /// <returns></returns>
        protected ComputeKeyMacResult ComputeMac(TOtherPartySharedInfo otherPartyInformation,
            BitString derivedKeyingMaterial)
        {
            if (SchemeParameters.KasMode == KasMode.KdfNoKc)
            {
                return NoKeyConfirmation(otherPartyInformation, derivedKeyingMaterial);
            }

            return KeyConfirmation(otherPartyInformation, derivedKeyingMaterial);
        }

        /// <summary>
        /// Performs macing with NoKeyConfirmation logic
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <param name="derivedKeyingMaterial">The DKM from the KDF</param>
        /// <returns></returns>
        protected ComputeKeyMacResult NoKeyConfirmation(TOtherPartySharedInfo otherPartyInformation, BitString derivedKeyingMaterial)
        {
            // No key confirmation nonce provided by party u.
            var noKeyConfirmationNonce = SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                ? NoKeyConfirmationNonce
                : otherPartyInformation.NoKeyConfirmationNonce;

            var noKeyConfirmationParameters = GetNoKeyConfirmationParameters(derivedKeyingMaterial, noKeyConfirmationNonce);

            var noKeyConfirmationInstance = NoKeyConfirmationFactory.GetInstance(noKeyConfirmationParameters);

            return noKeyConfirmationInstance.ComputeMac();
        }

        /// <summary>
        /// Gets the <see cref="NoKeyConfirmationParameters"/> for use in <see cref="INoKeyConfirmationFactory"/>
        /// </summary>
        /// <param name="derivedKeyingMaterial">The derived keying material (dkm) plugged into a MAC function H(dkm, macData)</param>
        /// <param name="noKeyConfirmationNonce">The nonce used as a party of macData in H(dkm, macData)</param>
        /// <returns></returns>
        protected INoKeyConfirmationParameters GetNoKeyConfirmationParameters(BitString derivedKeyingMaterial, BitString noKeyConfirmationNonce)
        {
            if (MacParameters.MacType == KeyAgreementMacType.AesCcm)
            {
                return new NoKeyConfirmationParameters(
                    MacParameters.MacType,
                    MacParameters.MacLength,
                    derivedKeyingMaterial,
                    noKeyConfirmationNonce,
                    MacParameters.CcmNonce
                );
            }

            return new NoKeyConfirmationParameters(
                MacParameters.MacType,
                MacParameters.MacLength,
                derivedKeyingMaterial,
                noKeyConfirmationNonce
            );
        }

        /// <summary>
        /// Performs macing with KeyConfirmation logic
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <param name="derivedKeyingMaterial">The DKM from the KDF</param>
        /// <returns></returns>
        protected ComputeKeyMacResult KeyConfirmation(TOtherPartySharedInfo otherPartyInformation, BitString derivedKeyingMaterial)
        {
            var keyConfirmationParameters = GetKeyConfirmationParameters(otherPartyInformation, derivedKeyingMaterial);

            var keyConfirmationInstance = KeyConfirmationFactory.GetInstance(keyConfirmationParameters);

            return keyConfirmationInstance.ComputeMac();
        }

        /// <summary>
        /// Gets the <see cref="KeyConfirmationParameters"/> for use in <see cref="IKeyConfirmationFactory"/>
        /// </summary>
        /// <param name="otherPartyInformation">The other party's information used in construction of the macData</param>
        /// <param name="derivedKeyingMaterial">The derived keying material (dkm) plugged into a MAC function H(dkm, macData)</param>
        /// <returns></returns>
        protected IKeyConfirmationParameters GetKeyConfirmationParameters(TOtherPartySharedInfo otherPartyInformation, BitString derivedKeyingMaterial)
        {
            var thisPartyEphemeralPublicKeyOrNonce =
                GetEphemeralKeyOrNonce(EphemeralKeyPair, EphemeralNonce, DkmNonce);
            var otherPartyEphemeralPublicKeyOrNonce =
                GetEphemeralKeyOrNonce(otherPartyInformation.EphemeralPublicKey, otherPartyInformation.EphemeralNonce, otherPartyInformation.DkmNonce);

            if (MacParameters.MacType == KeyAgreementMacType.AesCcm)
            {
                return new KeyConfirmationParameters(
                    SchemeParameters.KeyAgreementRole,
                    SchemeParameters.KeyConfirmationRole,
                    SchemeParameters.KeyConfirmationDirection,
                    MacParameters.MacType,
                    KdfParameters.KeyLength,
                    MacParameters.MacLength,
                    SchemeParameters.ThisPartyId,
                    otherPartyInformation.PartyId,
                    thisPartyEphemeralPublicKeyOrNonce,
                    otherPartyEphemeralPublicKeyOrNonce,
                    derivedKeyingMaterial,
                    MacParameters.CcmNonce
                );
            }

            return new KeyConfirmationParameters(
                SchemeParameters.KeyAgreementRole,
                SchemeParameters.KeyConfirmationRole,
                SchemeParameters.KeyConfirmationDirection,
                MacParameters.MacType,
                KdfParameters.KeyLength,
                MacParameters.MacLength,
                SchemeParameters.ThisPartyId,
                otherPartyInformation.PartyId,
                thisPartyEphemeralPublicKeyOrNonce,
                otherPartyEphemeralPublicKeyOrNonce,
                derivedKeyingMaterial
            );
        }
        #endregion protected implemented methods

        #region abstract methods to be implemented in extension DSA classes (Ffc/Ecc)
        /// <summary>
        /// Returns the sha instance to be used in DSA.
        /// </summary>
        /// <returns></returns>
        protected abstract ISha GetShaInstanceFromDsa();
        
        /// <summary>
        /// Ensures keys are valid according to assurances selected
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <returns></returns>
        protected abstract bool KeyValidityChecks(TOtherPartySharedInfo otherPartyInformation);

        /// <summary>
        /// Generate a set of domain parameters
        /// </summary>
        protected abstract void GenerateDomainParameters();

        /// <summary>
        /// Returns the ephemeral key or ephemeral nonce,
        /// dependant on which is valid in the provided parameters
        /// </summary>
        /// <param name="ephemeralPublicKey">The ephemeral public key</param>
        /// <param name="ephemeralNonce">the ephemeral nonce</param>
        /// <param name="dkmNonce">The dkm nonce (can be used in place of ephemeral nonce)</param>
        /// <returns></returns>
        protected abstract BitString GetEphemeralKeyOrNonce(TKeyPair ephemeralPublicKey, BitString ephemeralNonce, BitString dkmNonce);
        #endregion abstract methods to be implemented in extension DSA classes (Ffc/Ecc)

        #region scheme specific
        /// <summary>
        /// Generates key pairs and nonce information specific to the scheme selected
        /// </summary>
        protected abstract void GenerateKasKeyNonceInformation();

        /// <summary>
        /// Computes a shared secret based off of party U and party V inputs based on the scheme
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <returns></returns>
        protected abstract BitString ComputeSharedSecret(TOtherPartySharedInfo otherPartyInformation);
        #endregion scheme specific
    }
}