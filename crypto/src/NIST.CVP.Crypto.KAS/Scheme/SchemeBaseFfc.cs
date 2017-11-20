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
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBaseFfc 
        : SchemeBase<
            SchemeParametersBase<
                FfcParameterSet, 
                FfcScheme
            >, 
            FfcParameterSet, 
            FfcScheme, 
            OtherPartySharedInformation<
                FfcDomainParameters, 
                FfcKeyPair
            >, 
            FfcDomainParameters, 
            FfcKeyPair
        >
    {
        protected IDsaFfc Dsa;
        
        protected SchemeBaseFfc(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory,
            INoKeyConfirmationFactory noKeyConfirmationFactory,
            IOtherInfoFactory<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> otherInfoFactory,
            IEntropyProvider entropyProvider,
            SchemeParametersBase<FfcParameterSet, FfcScheme> schemeParameters,
            KdfParameters kdfParameters,
            MacParameters macParameters
        )
            : base(
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
            Dsa = dsa;
        }

        /// <inheritdoc />
        public override int OtherInputLength => 240;

        /// <inheritdoc />
        public override OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> ReturnPublicInfoThisParty()
        {
            if (!ThisPartyKeysGenerated)
            {
                GenerateKasKeyNonceInformation();
            }

            return new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                DomainParameters,
                SchemeParameters.ThisPartyId,
                StaticKeyPair,
                EphemeralKeyPair,
                EphemeralNonce,
                DkmNonce,
                NoKeyConfirmationNonce
            );
        }

        /// <inheritdoc />
        protected override ISha GetShaInstanceFromDsa()
        {
            return Dsa.Sha;
        }

        /// <inheritdoc />
        protected override bool KeyValidityChecks(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> otherPartyInformation)
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
                    otherPartyInformation.StaticPublicKey != null)
                {
                    KeyValidationHelper.PerformFfcPublicKeyValidation(
                        DomainParameters.P,
                        DomainParameters.Q,
                        otherPartyInformation.StaticPublicKey.PublicKeyY,
                        true
                    );
                }

                // When fullval, and the other party provides a ephemeral public key
                if (SchemeParameters.KasAssurances.HasFlag(KasAssurance.FullVal) &&
                    otherPartyInformation.EphemeralPublicKey != null)
                {
                    KeyValidationHelper.PerformFfcPublicKeyValidation(
                        DomainParameters.P,
                        DomainParameters.Q,
                        otherPartyInformation.EphemeralPublicKey.PublicKeyY,
                        true
                    );
                }

                // When using DpVal or KeyRegen, with a static key, 
                // perform private static key validation
                if ((SchemeParameters.KasAssurances.HasFlag(KasAssurance.DpVal) ||
                    SchemeParameters.KasAssurances.HasFlag(KasAssurance.KeyRegen)) &&
                        StaticKeyPair != null)
                {
                    if (!Dsa.ValidateKeyPair(DomainParameters, StaticKeyPair).Success)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        protected override void GenerateDomainParameters()
        {
            var paramDetails = ParameterSetDetails.GetDetailsForFfcParameterSet(SchemeParameters.ParameterSet);

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

        /// <inheritdoc />
        protected override BitString GetEphemeralKeyOrNonce(FfcKeyPair ephemeralPublicKey, BitString ephemeralNonce)
        {
            if (ephemeralPublicKey != null && ephemeralPublicKey?.PublicKeyY != 0)
            {
                var ephemKey = new BitString(ephemeralPublicKey.PublicKeyY);

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