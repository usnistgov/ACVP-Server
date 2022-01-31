using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Builders
{
    public class SecretKeyingMaterialBuilder : ISecretKeyingMaterialBuilder
    {
        private IDsaDomainParameters _domainParameters;
        private IDsaKeyPair _ephemeralKey;
        private IDsaKeyPair _staticKey;
        private BitString _ephemeralNonce;
        private BitString _dkmNonce;
        private BitString _partyId;

        public ISecretKeyingMaterialBuilder WithDomainParameters(IDsaDomainParameters value)
        {
            _domainParameters = value;
            return this;
        }

        public ISecretKeyingMaterialBuilder WithEphemeralKey(IDsaKeyPair value)
        {
            _ephemeralKey = value;
            return this;
        }

        public ISecretKeyingMaterialBuilder WithStaticKey(IDsaKeyPair value)
        {
            _staticKey = value;
            return this;
        }

        public ISecretKeyingMaterialBuilder WithEphemeralNonce(BitString value)
        {
            _ephemeralNonce = value;
            return this;
        }

        public ISecretKeyingMaterialBuilder WithDkmNonce(BitString value)
        {
            _dkmNonce = value;
            return this;
        }

        public ISecretKeyingMaterialBuilder WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        public ISecretKeyingMaterial Build(KasScheme scheme, KasMode kasMode, KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole keyConfirmationRole, KeyConfirmationDirection keyConfirmationDirection)
        {
            var schemeRequirements = KasEnumMapping.GetSchemeRequirements(
                scheme,
                kasMode,
                thisPartyKeyAgreementRole,
                keyConfirmationRole,
                keyConfirmationDirection);

            ValidateDomainParameters(schemeRequirements.requirments, _domainParameters);
            ValidateEphemeralKey(schemeRequirements.requirments, _ephemeralKey);
            ValidateStaticKey(schemeRequirements.requirments, _staticKey);
            ValidateEphemeralNonce(schemeRequirements.requirments, _ephemeralNonce);
            ValidateDkmNonce(schemeRequirements.requirments, _dkmNonce);
            ValidatePartyId(schemeRequirements.requirments, _partyId);
            ValidateConsistentAlgorithm(schemeRequirements.kasAlgo, schemeRequirements.requirments, _domainParameters, _ephemeralKey, _staticKey);

            return new SecretKeyingMaterial()
            {
                KasAlgorithm = schemeRequirements.kasAlgo,
                DkmNonce = _dkmNonce,
                DomainParameters = _domainParameters,
                EphemeralNonce = _ephemeralNonce,
                EphemeralKeyPair = _ephemeralKey,
                StaticKeyPair = _staticKey,
                PartyId = _partyId,
            };
        }

        private void ValidateDomainParameters(SchemeKeyNonceGenRequirement schemeRequirements, IDsaDomainParameters domainParameters)
        {
            if (domainParameters == null)
                throw new ArgumentNullException(nameof(domainParameters));
        }

        private void ValidateEphemeralKey(SchemeKeyNonceGenRequirement schemeRequirements, IDsaKeyPair ephemeralKey)
        {
            if (schemeRequirements.GeneratesEphemeralKeyPair && ephemeralKey == null)
                throw new ArgumentNullException(nameof(ephemeralKey));
        }

        private void ValidateStaticKey(SchemeKeyNonceGenRequirement schemeRequirements, IDsaKeyPair staticKey)
        {
            if (schemeRequirements.GeneratesStaticKeyPair && staticKey == null)
                throw new ArgumentNullException(nameof(staticKey));
        }

        private void ValidateEphemeralNonce(SchemeKeyNonceGenRequirement schemeRequirements, BitString ephemeralNonce)
        {
            if (schemeRequirements.GeneratesEphemeralNonce && ephemeralNonce == null)
                throw new ArgumentNullException(nameof(ephemeralNonce));
        }

        private void ValidateDkmNonce(SchemeKeyNonceGenRequirement schemeRequirements, BitString dkmNonce)
        {
            if (schemeRequirements.GeneratesDkmNonce && dkmNonce == null)
                throw new ArgumentNullException(nameof(dkmNonce));
        }

        private void ValidatePartyId(SchemeKeyNonceGenRequirement schemeRequirements, BitString partyId)
        {
            if (schemeRequirements.KasMode != KasMode.NoKdfNoKc && partyId == null)
                throw new ArgumentNullException(nameof(partyId));
        }

        private void ValidateConsistentAlgorithm(KasAlgorithm schemeRequirementsKasAlgo, SchemeKeyNonceGenRequirement schemeRequirementsRequirments, IDsaDomainParameters domainParameters, IDsaKeyPair ephemeralKey, IDsaKeyPair staticKey)
        {
            switch (schemeRequirementsKasAlgo)
            {
                case KasAlgorithm.Ffc:
                    if (domainParameters.GetType() != typeof(FfcDomainParameters))
                    {
                        throw new ArgumentException($"{nameof(domainParameters)} expected type {typeof(FfcDomainParameters)} got {domainParameters.GetType()}");
                    }

                    if (ephemeralKey != null && ephemeralKey.GetType() != typeof(FfcKeyPair))
                    {
                        throw new ArgumentException($"{nameof(ephemeralKey)} expected type {typeof(FfcKeyPair)} got {ephemeralKey.GetType()}");
                    }

                    if (staticKey != null && staticKey.GetType() != typeof(FfcKeyPair))
                    {
                        throw new ArgumentException($"{nameof(staticKey)} expected type {typeof(FfcKeyPair)} got {staticKey.GetType()}");
                    }
                    break;
                case KasAlgorithm.Ecc:
                    if (domainParameters.GetType() != typeof(EccDomainParameters))
                    {
                        throw new ArgumentException($"{nameof(domainParameters)} expected type {typeof(EccDomainParameters)} got {domainParameters.GetType()}");
                    }

                    if (ephemeralKey != null && ephemeralKey.GetType() != typeof(EccKeyPair))
                    {
                        throw new ArgumentException($"{nameof(ephemeralKey)} expected type {typeof(EccKeyPair)} got {ephemeralKey.GetType()}");
                    }

                    if (staticKey != null && staticKey.GetType() != typeof(EccKeyPair))
                    {
                        throw new ArgumentException($"{nameof(staticKey)} expected type {typeof(EccKeyPair)} got {staticKey.GetType()}");
                    }
                    break;
                default:
                    throw new ArgumentException(nameof(schemeRequirementsKasAlgo));
            }
        }
    }
}
