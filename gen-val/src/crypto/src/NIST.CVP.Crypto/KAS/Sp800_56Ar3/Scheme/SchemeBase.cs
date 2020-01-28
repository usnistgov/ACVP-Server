using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme
{
    internal abstract class SchemeBase : IScheme
    {
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly FixedInfoParameter _fixedInfoParameter;
        private readonly IKdfParameter _kdfParameter;
        private readonly IKdfFactory _kdfFactory;
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly MacParameters _keyConfirmationParameter;
        
        protected SchemeBase(
            SchemeParameters schemeParameters, 
            ISecretKeyingMaterial thisPartyKeyingMaterial,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters keyConfirmationParameter)
        {
            SchemeParameters = schemeParameters;
            ThisPartyKeyingMaterial = thisPartyKeyingMaterial;
            _fixedInfoFactory = fixedInfoFactory;
            _fixedInfoParameter = fixedInfoParameter;
            _kdfFactory = kdfFactory;
            _kdfParameter = kdfParameter;
            _keyConfirmationFactory = keyConfirmationFactory;
            _keyConfirmationParameter = keyConfirmationParameter;
        }
        
        public SchemeParameters SchemeParameters { get; }
        public ISecretKeyingMaterial ThisPartyKeyingMaterial { get; set; }
        public KeyAgreementResult ComputeResult(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            var z = ComputeSharedSecret(otherPartyKeyingMaterial);

            var keyingMaterialPartyU = SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                ? ThisPartyKeyingMaterial
                : otherPartyKeyingMaterial;
            var keyingMaterialPartyV = SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV
                ? ThisPartyKeyingMaterial
                : otherPartyKeyingMaterial;
            
            if (_kdfParameter == null)
            {
                return new KeyAgreementResult(keyingMaterialPartyU, keyingMaterialPartyV, z);
            }
            
            var fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);
            var dkm = DeriveKey(otherPartyKeyingMaterial, z, fixedInfo);

            if (_keyConfirmationFactory == null)
            {
                return new KeyAgreementResult(keyingMaterialPartyU, keyingMaterialPartyV, z, fixedInfo, dkm);
            }
            
            var keyConfirmationKey = dkm.GetMostSignificantBits(_keyConfirmationParameter.KeyLength);
            var newKeyToTransport = dkm.MSBSubstring(
                _keyConfirmationParameter.KeyLength - 1,dkm.BitLength - _keyConfirmationParameter.KeyLength);
            var keyConfirmationResult = KeyConfirmation(otherPartyKeyingMaterial, keyConfirmationKey);
            
            return new KeyAgreementResult(
                keyingMaterialPartyU, keyingMaterialPartyV, z, fixedInfo, newKeyToTransport, 
                keyConfirmationKey, keyConfirmationResult.MacData, keyConfirmationResult.Mac);
        }

        /// <summary>
        /// Computes a shared secret Z using secret keying material from two parties.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other party secret keying material.</param>
        /// <returns>Shared secret Z.</returns>
        protected abstract BitString ComputeSharedSecret(ISecretKeyingMaterial otherPartyKeyingMaterial);

        /// <summary>
        /// Sets up the KDF parameter with the ephemeral data provided by the two parties to be used in the key derivation process.
        /// </summary>
        /// <param name="kdfParameter">The kdfParameter parameter in which to have its ephemeral data set.</param>
        /// <param name="otherPartyKeyingMaterial">The other party keying material.</param>
        /// <param name="z">The shared secret z.</param>
        private void SetKdfEphemeralData(IKdfParameter kdfParameter, ISecretKeyingMaterial otherPartyKeyingMaterial, BitString z)
        {
            BitString initiatorData = null;
            BitString responderData = null;

            kdfParameter.Z = z;
            
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    initiatorData = GetEphemeralDataFromKeyContribution(
                        ThisPartyKeyingMaterial);
                    responderData = GetEphemeralDataFromKeyContribution(
                        otherPartyKeyingMaterial);
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    initiatorData = GetEphemeralDataFromKeyContribution(
                        otherPartyKeyingMaterial);
                    responderData = GetEphemeralDataFromKeyContribution(
                        ThisPartyKeyingMaterial);
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }

            kdfParameter.SetEphemeralData(initiatorData, responderData);
        }

        /// <summary>
        /// Derives a key using a KDF in conjunction with a shared secret Z, and <see cref="ISecretKeyingMaterial"/> from two parties.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other party keying material.</param> 
        /// <param name="z">The shared secret.</param>
        /// <param name="fixedInfo">The constructed fixed information from ephemeral data provided by each party.</param>
        /// <returns>The derived keying material.</returns>
        private BitString DeriveKey(ISecretKeyingMaterial otherPartyKeyingMaterial, BitString z, BitString fixedInfo)
        {
            SetKdfEphemeralData(_kdfParameter, otherPartyKeyingMaterial, z);
            return _kdfFactory.GetKdf().DeriveKey(_kdfParameter, fixedInfo).DerivedKey;
        }
        
        /// <summary>
        /// Gets the fixed info to be used as an input to a KDF.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other party keying material.</param>
        /// <returns></returns>
        private BitString GetFixedInfo(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            var fixedInfo = _fixedInfoFactory.Get();

            var thisPartyFixedInfo = GetPartyFixedInfo(ThisPartyKeyingMaterial);
            var otherPartyFixedInfo = GetPartyFixedInfo(otherPartyKeyingMaterial);

            _fixedInfoParameter.SetFixedInfo(
                SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                    ? thisPartyFixedInfo
                    : otherPartyFixedInfo,
                SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV
                    ? thisPartyFixedInfo
                    : otherPartyFixedInfo
            );
            
            return fixedInfo.Get(_fixedInfoParameter);
        }
        
        /// <summary>
        /// Get the <see cref="PartyFixedInfo"/> as it pertains to the provided <see cref="ISecretKeyingMaterial"/>
        /// for the specified <see cref="KeyAgreementRole"/>.
        /// </summary>
        /// <param name="secretKeyingMaterial">The secret keying material for the party.</param>
        /// <param name="keyAgreementRole">The parties key agreement role.</param>
        /// <returns></returns>
        private PartyFixedInfo GetPartyFixedInfo(ISecretKeyingMaterial secretKeyingMaterial)
        {
            return new PartyFixedInfo(secretKeyingMaterial.PartyId, GetEphemeralDataFromKeyContribution(secretKeyingMaterial));
        }
        
        /// <summary>
        /// The ephemeral data provided by a party's secret keying material to be used as a part of fixedInfo construction. 
        /// </summary>
        /// <param name="secretKeyingMaterial">a party's secret keying material</param>
        /// <returns>The ephemeral data for a party's contribution to fixedInfo.</returns>
        protected abstract BitString GetEphemeralDataFromKeyContribution(ISecretKeyingMaterial secretKeyingMaterial);
        
        /// <summary>
        /// Performs key confirmation using both parties contributions to the key establishment. 
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties contributions to the scheme.</param>
        /// <param name="keyToTransport">The key that was derived in the KAS or KTS scheme.</param>
        /// <returns></returns>
        private ComputeKeyMacResult KeyConfirmation(ISecretKeyingMaterial otherPartyKeyingMaterial, BitString keyToTransport)
        {
            var keyConfParam = GetKeyConfirmationParameters(otherPartyKeyingMaterial, keyToTransport);

            var keyConfirmation = _keyConfirmationFactory.GetInstance(keyConfParam);
            return keyConfirmation.ComputeMac();
        }

        /// <summary>
        /// Generate the <see cref="IKeyConfirmationParameters"/> based on the two parties information.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties keying information.</param>
        /// <param name="keyToTransport">The derived keying material.</param>
        /// <returns></returns>
        private IKeyConfirmationParameters GetKeyConfirmationParameters(ISecretKeyingMaterial otherPartyKeyingMaterial, BitString keyToTransport)
        {
            var thisPartyEphemData =
                GetEphemeralDataFromKeyContribution(ThisPartyKeyingMaterial);
            var otherPartyEphemData =
                GetEphemeralDataFromKeyContribution(otherPartyKeyingMaterial);
            
            return new KeyConfirmationParameters(
                SchemeParameters.KeyAgreementRole,
                SchemeParameters.KeyConfirmationRole,
                SchemeParameters.KeyConfirmationDirection,
                _keyConfirmationParameter.MacType,
                _keyConfirmationParameter.KeyLength,
                _keyConfirmationParameter.MacLength,
                ThisPartyKeyingMaterial.PartyId,
                otherPartyKeyingMaterial.PartyId,
                thisPartyEphemData,
                otherPartyEphemData,
                keyToTransport
                );
        }
    }
}