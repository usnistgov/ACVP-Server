using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ifc
{
    public class IfcSecretKeyingMaterialBuilder : IIfcSecretKeyingMaterialBuilder
    {
        private KeyPair _key;
        private BitString _z;
        private BitString _dkmNonce;
        private BitString _partyId;
        private BitString _c;
        private BitString _k;

        public IIfcSecretKeyingMaterialBuilder WithKey(KeyPair value)
        {
            _key = value;
            return this;
        }

        public IIfcSecretKeyingMaterialBuilder WithZ(BitString value)
        {
            _z = value;
            return this;
        }

        public IIfcSecretKeyingMaterialBuilder WithDkmNonce(BitString value)
        {
            _dkmNonce = value;
            return this;
        }

        public IIfcSecretKeyingMaterialBuilder WithPartyId(BitString value)
        {
            _partyId = value;
            return this;
        }

        public IIfcSecretKeyingMaterialBuilder WithC(BitString value)
        {
            _c = value;
            return this;
        }

        public IIfcSecretKeyingMaterialBuilder WithK(BitString value)
        {
            _k = value;
            return this;
        }

        public IIfcSecretKeyingMaterial Build(IfcScheme scheme, KasMode kasMode, KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole keyConfirmationRole, KeyConfirmationDirection keyConfirmationDirection,
            bool shouldValidateContributions = true)
        {
            if (shouldValidateContributions)
            {
                var generationRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    scheme, kasMode, thisPartyKeyAgreementRole, keyConfirmationRole, keyConfirmationDirection);

                ValidateNonce(generationRequirements);
                ValidateKey(generationRequirements);
                ValidatePartyId(generationRequirements);
                ValidateK(generationRequirements, scheme);
            }

            return new IfcSecretKeyingMaterial()
            {
                C = _c,
                DkmNonce = _dkmNonce,
                K = _k,
                Key = _key,
                PartyId = _partyId,
                Z = _z,
            };
        }

        /// <summary>
        /// Validates that a nonce is provided when required.
        /// </summary>
        /// <param name="generationRequirements">The generation requirements for this party and scheme.</param>
        /// <exception cref="ArgumentNullException">Throw when required value is not provided.</exception>
        private void ValidateNonce(SchemeKeyNonceGenRequirement generationRequirements)
        {
            if (generationRequirements.KasMode == KasMode.NoKdfNoKc)
            {
                return;
            }

            if (generationRequirements.GeneratesDkmNonce && _dkmNonce?.BitLength == 0)
            {
                throw new ArgumentNullException($"{nameof(_dkmNonce)} cannot be null with this party's contributions.");
            }
        }

        /// <summary>
        /// Validates that a key is provided when required.
        /// When "Generates" is required, ensure you have at a minimum the public key.
        /// </summary>
        /// <param name="generationRequirements">The generation requirements for this party and scheme.</param>
        /// <exception cref="ArgumentNullException">Throw when required value is not provided.</exception>
        private void ValidateKey(SchemeKeyNonceGenRequirement generationRequirements)
        {
            if (generationRequirements.GeneratesEphemeralKeyPair && _key?.PubKey == null)
            {
                throw new ArgumentException($"{nameof(_key.PubKey)} cannot be null with this party's contributions.");
            }
        }

        /// <summary>
        /// Validates a party id is provided.
        /// </summary>
        /// <param name="generationRequirements">The generation requirements for this party and scheme.</param>
        /// <exception cref="ArgumentNullException">Throw when required value is not provided.</exception>
        private void ValidatePartyId(SchemeKeyNonceGenRequirement generationRequirements)
        {
            if (generationRequirements.KasMode == KasMode.NoKdfNoKc)
            {
                return;
            }

            if (_partyId?.BitLength == 0)
            {
                throw new ArgumentNullException($"{nameof(_partyId)} cannot be null.");
            }
        }

        /// <summary>
        /// Validates the "k" is provided when party U for KTS scheme.
        /// </summary>
        /// <param name="generationRequirements">The generation requirements for this party and scheme.</param>
        /// <param name="scheme">The scheme utilized for the KAS/KTS.</param>
        /// <exception cref="ArgumentNullException">Throw when required value is not provided.</exception>
        private void ValidateK(SchemeKeyNonceGenRequirement generationRequirements, IfcScheme scheme)
        {
            var ktsSchemes = new[] { IfcScheme.Kts_oaep_basic, IfcScheme.Kts_oaep_partyV_keyConfirmation };

            if (generationRequirements.ThisPartyKasRole == KeyAgreementRole.InitiatorPartyU && ktsSchemes.Contains(scheme) && _k?.BitLength == 0)
            {
                throw new ArgumentNullException($"{nameof(_key.PubKey)} cannot be null for this party and scheme.");
            }
        }
    }
}
