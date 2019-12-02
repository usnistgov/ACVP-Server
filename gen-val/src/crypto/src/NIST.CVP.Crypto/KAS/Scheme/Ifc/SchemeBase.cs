using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    internal abstract class SchemeBase : ISchemeIfc
    {
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly IFixedInfoFactory _fixedInfoFactory;
        private readonly FixedInfoParameter _fixedInfoParameter;
        private readonly MacParameters _macParameters;
        
        private IIfcSecretKeyingMaterial _thisPartyKeyingMaterial;
        private bool _isThisPartyKeyingMaterialInitialized;

        protected readonly IIfcSecretKeyingMaterialBuilder _thisPartyKeyingMaterialBuilder;
        protected readonly IEntropyProvider EntropyProvider;
        
        protected SchemeBase(
            IEntropyProvider entropyProvider,
            SchemeParametersIfc schemeParameters,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters)
        {
            EntropyProvider = entropyProvider;
            SchemeParameters = schemeParameters;
            _fixedInfoFactory = fixedInfoFactory;
            _fixedInfoParameter = fixedInfoParameter;
            _thisPartyKeyingMaterialBuilder = thisPartyKeyingMaterialBuilder;
            _keyConfirmationFactory = keyConfirmationFactory;
            _macParameters = macParameters;
        }

        public SchemeParametersIfc SchemeParameters { get; }

        public IIfcSecretKeyingMaterial ThisPartyKeyingMaterial
        {
            get
            {
                if (_isThisPartyKeyingMaterialInitialized)
                {
                    return _thisPartyKeyingMaterial;
                }
                
                throw new NotSupportedException("This party keying material not yet initialized.");
            }
            set
            {
                _isThisPartyKeyingMaterialInitialized = true;
                _thisPartyKeyingMaterial = value;
            }
        }

        public void InitializeThisPartyKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            _isThisPartyKeyingMaterialInitialized = true;

            BuildKeyingMaterialThisParty(_thisPartyKeyingMaterialBuilder, otherPartyKeyingMaterial);
            
            _thisPartyKeyingMaterial = _thisPartyKeyingMaterialBuilder.Build(
                SchemeParameters.KasAlgoAttributes.Scheme, 
                SchemeParameters.KasMode,
                SchemeParameters.KeyAgreementRole,
                SchemeParameters.KeyConfirmationRole,
                SchemeParameters.KeyConfirmationDirection
                );
        }

        protected abstract void BuildKeyingMaterialThisParty(
            IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IIfcSecretKeyingMaterial otherPartyKeyingMaterial);

        public KasIfcResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            if (!_isThisPartyKeyingMaterialInitialized)
            {
                InitializeThisPartyKeyingMaterial(otherPartyKeyingMaterial);
            }
            
            var keyToTransport = GetKeyingMaterial(otherPartyKeyingMaterial);

            var keyingMaterialPartyU = SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                ? ThisPartyKeyingMaterial
                : otherPartyKeyingMaterial;
            var keyingMaterialPartyV = SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV
                ? ThisPartyKeyingMaterial
                : otherPartyKeyingMaterial;
            
            // No key confirmation, return the whole key
            if (_keyConfirmationFactory == null)
            {
                return new KasIfcResult(keyingMaterialPartyU, keyingMaterialPartyV, keyToTransport);
            }

            var keyConfirmationKey = keyToTransport.GetMostSignificantBits(_macParameters.KeyLength);
            var newKeyToTransport = keyToTransport.MSBSubstring(
                _macParameters.KeyLength - 1,keyToTransport.BitLength - _macParameters.KeyLength);
            var keyConfirmationResult = KeyConfirmation(otherPartyKeyingMaterial, keyConfirmationKey);
            
            return new KasIfcResult(
                keyingMaterialPartyU, keyingMaterialPartyV, 
                newKeyToTransport, keyConfirmationKey, keyConfirmationResult.MacData, keyConfirmationResult.Mac);
        }

        /// <summary>
        /// Performs key confirmation using both parties contributions to the key establishment. 
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other parties contributions to the scheme.</param>
        /// <param name="keyToTransport">The key that was derived in the KAS or KTS scheme.</param>
        /// <returns></returns>
        private ComputeKeyMacResult KeyConfirmation(IIfcSecretKeyingMaterial otherPartyKeyingMaterial, BitString keyToTransport)
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
        private IKeyConfirmationParameters GetKeyConfirmationParameters(IIfcSecretKeyingMaterial otherPartyKeyingMaterial, BitString keyToTransport)
        {
            var thisPartyEphemData =
                GetEphemeralDataFromKeyContribution(ThisPartyKeyingMaterial, SchemeParameters.KeyAgreementRole, false);
            var otherPartyEphemData =
                GetEphemeralDataFromKeyContribution(otherPartyKeyingMaterial,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(SchemeParameters.KeyAgreementRole),
                    false);
            
            return new KeyConfirmationParameters(
                SchemeParameters.KeyAgreementRole,
                SchemeParameters.KeyConfirmationRole,
                SchemeParameters.KeyConfirmationDirection,
                _macParameters.MacType,
                _macParameters.KeyLength,
                _macParameters.MacLength,
                ThisPartyKeyingMaterial.PartyId,
                otherPartyKeyingMaterial.PartyId,
                thisPartyEphemData,
                otherPartyEphemData,
                keyToTransport
                );
        }

        /// <summary>
        /// Get the FixedInfo BitString for use in KDFs and KTS.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial">The other party keying material</param>
        /// <param name="excludeEphemeralData">Should the ephemeral data be excluded? (Used for KTS fixed info generation)</param>
        /// <returns></returns>
        protected BitString GetFixedInfo(IIfcSecretKeyingMaterial otherPartyKeyingMaterial, bool excludeEphemeralData = false)
        {
            var fixedInfo = _fixedInfoFactory.Get();

            var thisPartyFixedInfo = GetPartyFixedInfo(ThisPartyKeyingMaterial, SchemeParameters.KeyAgreementRole, excludeEphemeralData);
            var otherPartyRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(SchemeParameters.KeyAgreementRole);
            var otherPartyFixedInfo = GetPartyFixedInfo(otherPartyKeyingMaterial, otherPartyRole, excludeEphemeralData);

            _fixedInfoParameter.FixedInfoPartyU = SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU
                ? thisPartyFixedInfo
                : otherPartyFixedInfo;
            _fixedInfoParameter.FixedInfoPartyV = SchemeParameters.KeyAgreementRole == KeyAgreementRole.ResponderPartyV
                ? thisPartyFixedInfo
                : otherPartyFixedInfo;
            
            return fixedInfo.Get(_fixedInfoParameter);
        }

        /// <summary>
        /// Get the <see cref="PartyFixedInfo"/> as it pertains to the provided <see cref="IIfcSecretKeyingMaterial"/>
        /// for the specified <see cref="KeyAgreementRole"/>.
        /// </summary>
        /// <param name="secretKeyingMaterial">The secret keying material for the party.</param>
        /// <param name="keyAgreementRole">The parties key agreement role.</param>
        /// <param name="excludeEphemeralData">Should the ephemeral data be excluded? (Used for KTS fixed info generation)</param>
        /// <returns></returns>
        private PartyFixedInfo GetPartyFixedInfo(IIfcSecretKeyingMaterial secretKeyingMaterial, KeyAgreementRole keyAgreementRole, bool excludeEphemeralData)
        {
            return new PartyFixedInfo(secretKeyingMaterial.PartyId, GetEphemeralDataFromKeyContribution(secretKeyingMaterial, keyAgreementRole, excludeEphemeralData));
        }
       
        /// <summary>
        /// Creates/Gets/Recovers a key for a KAS/KTS scheme.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial"></param>
        /// <returns></returns>
        protected abstract BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial);        
        
        /// <summary>
        /// The ephemeral data can be composed of any of C, CU, CV, NV, depending on the party, and scheme
        ///
        /// Scheme      Party          Ephemeral Data
        /// KAS1        Party U        C
        /// KAS1        Party V        NV
        /// KAS2        Party U        CU
        /// KAS2        Party V        CV
        /// KTS         Party U        C
        /// KTS         Party V        null
        /// </summary>
        /// <param name="secretKeyingMaterial">a party's secret keying material</param>
        /// <param name="keyAgreementRole">a party's key agreement role</param>
        /// <param name="excludeEphemeralData">Should the ephemeral data be excluded? (Used for KTS fixed info generation)</param>
        /// <returns></returns>
        protected abstract BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial, KeyAgreementRole keyAgreementRole, bool excludeEphemeralData);

    }
}