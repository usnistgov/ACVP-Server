using System;
using System.Numerics;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBaseFfc : IScheme
    {
        protected IDsaFfc Dsa;
        protected IKdfFactory KdfFactory;
        protected IKeyConfirmationFactory KeyConfirmationFactory;
        protected INoKeyConfirmationFactory NoKeyConfirmationFactory;
        protected IOtherInfoFactory OtherInfoFactory;
        protected IEntropyProvider EntropyProvider;
        protected KdfParameters KdfParameters;
        protected MacParameters MacParameters;

        protected SchemeBaseFfc(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParameters schemeParameters,
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
            SchemeParameters = schemeParameters;
            KdfParameters = kdfParameters;
            MacParameters = macParameters;
        }

        /// <inheritdoc />
        public int OtherInputLength => 240;

        /// <inheritdoc />
        public SchemeParameters SchemeParameters { get; protected set; }
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
                SchemeParameters.ThisPartyId,
                StaticKeyPair?.PublicKeyY ?? 0,
                EphemeralKeyPair?.PublicKeyY ?? 0,
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

            // Perform keychecks
            if (!KeyChecks(otherPartyInformation))
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
                var tag = Dsa.Sha.HashMessage(z);

                return new KasResult(z, tag.Digest);
            }

            // Get other information
            var oi = GenerateOtherInformation(otherPartyInformation).GetOtherInfo();

            // Get keying material
            var kdf = KdfFactory.GetInstance(KdfHashMode.Sha, Dsa.Sha.HashFunction);
            var dkm = kdf.DeriveKey(z, KdfParameters.KeyLength, oi);

            // Perform no/key confirmation
            var computedKeyMac = ComputeMac(otherPartyInformation, dkm.DerivedKey);

            return new KasResult(z, oi, dkm.DerivedKey, computedKeyMac.MacData, computedKeyMac.Mac);
        }

        private bool KeyChecks(FfcSharedInformation otherPartyInformation)
        {
            try
            {
                // When KeyPairGen or FullVal, check the static public key
                if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.KeyPairGen) ||
                    SchemeParameters.KasAssurances.HasFlag(KasAssurance.FullVal) &&
                    StaticKeyPair != null)
                {
                    KeyValidationHelper.PerformFfcPublicKeyValidation(
                        DomainParameters.P,
                        DomainParameters.Q,
                        StaticKeyPair.PublicKeyY,
                        true
                    );
                }

                // When fullval, and the other party provides a static public key
                if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.FullVal) &&
                    otherPartyInformation.StaticPublicKey != 0)
                {
                    KeyValidationHelper.PerformFfcPublicKeyValidation(
                        DomainParameters.P,
                        DomainParameters.Q,
                        otherPartyInformation.StaticPublicKey,
                        true
                    );
                }

                // When fullval, and the other party provides a ephemeral public key
                if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.FullVal) &&
                    otherPartyInformation.EphemeralPublicKey != 0)
                {
                    KeyValidationHelper.PerformFfcPublicKeyValidation(
                        DomainParameters.P,
                        DomainParameters.Q,
                        otherPartyInformation.EphemeralPublicKey,
                        true
                    );
                }

                // When using DpVal or KeyRegen, with a static key, 
                // perform private static key validation
                if ((SchemeParameters.KasAssurances.HasFlag(KasAssurance.DpVal) ||
                    SchemeParameters.KasAssurances.HasFlag(KasAssurance.KeyRegen)) &&
                        StaticKeyPair != null)
                {
                    if (Dsa.ValidateKeyPair(DomainParameters, StaticKeyPair).Success)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generate a set of domain parameters
        /// </summary>
        protected void GenerateDomainParameters()
        {
            var paramDetails = FfcParameterSetDetails.GetDetailsForParameterSet(SchemeParameters.FfcParameterSet);

            SetDomainParameters(
                Dsa.GenerateDomainParameters(
                    new FfcDomainParametersGenerateRequest(
                        paramDetails.qLength,
                        paramDetails.pLength,
                        paramDetails.qLength,
                        Dsa.Sha.HashFunction.OutputLen,
                        BitString.One(), 
                        PrimeGenMode.Provable,
                        GeneratorGenMode.Canonical
                    )
                ).PqgDomainParameters);
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

        protected IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation)
        {
            return OtherInfoFactory.GetInstance(
                KdfParameters.OtherInfoPattern,
                OtherInputLength,
                SchemeParameters.KeyAgreementRole,
                ReturnPublicInfoThisParty(),
                otherPartyInformation
            );
        }

        /// <summary>
        /// Compute's the MAC of a key for both key confirmation and no key confirmation
        /// </summary>
        /// <param name="otherPartyInformation">The other party's public information</param>
        /// <param name="derivedKeyingMaterial">The derived keying material generated via KDF</param>
        /// <returns></returns>
        protected ComputeKeyMacResult ComputeMac(FfcSharedInformation otherPartyInformation,
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
        protected ComputeKeyMacResult NoKeyConfirmation(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial)
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
        private INoKeyConfirmationParameters GetNoKeyConfirmationParameters(BitString derivedKeyingMaterial, BitString noKeyConfirmationNonce)
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
        protected ComputeKeyMacResult KeyConfirmation(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial)
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
        private IKeyConfirmationParameters GetKeyConfirmationParameters(FfcSharedInformation otherPartyInformation, BitString derivedKeyingMaterial)
        {
            var thisPartyEphemeralPublicKeyOrNonce =
                GetEphemeralKeyOrNonce(EphemeralKeyPair?.PublicKeyY ?? 0, EphemeralNonce);
            var otherPartyEphemeralPublicKeyOrNonce = 
                GetEphemeralKeyOrNonce(otherPartyInformation.EphemeralPublicKey, otherPartyInformation.EphemeralNonce);

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

        /// <summary>
        /// Gets the ephemeral key or nonce from a EphemeralPublicKey and EphemeralNonce
        /// </summary>
        /// <param name="ephemeralPublicKey">The key to use (when not 0)</param>
        /// <param name="ephemeralNonce">The nonce to use (when the key is 0)</param>
        /// <returns></returns>
        private BitString GetEphemeralKeyOrNonce(BigInteger ephemeralPublicKey, BitString ephemeralNonce)
        {
            if (ephemeralPublicKey != 0)
            {
                var ephemKey = new BitString(ephemeralPublicKey);

                // Ensure mod 32
                if (ephemKey.BitLength % 32 != 0)
                {
                    ephemKey = BitString.ConcatenateBits(BitString.Zeroes(32 - ephemKey.BitLength % 32), ephemKey);
                }

                return ephemKey;
            }

            return ephemeralNonce;
        }
    }
}