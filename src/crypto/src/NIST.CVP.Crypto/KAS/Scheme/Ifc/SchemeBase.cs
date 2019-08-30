using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public abstract class SchemeBase : ISchemeIfc
    {
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;
        private readonly MacParameters _macParameters;

        protected SchemeBase(
            SchemeParametersIfc schemeParameters, 
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters)
        {
            SchemeParameters = schemeParameters;
            ThisPartyKeyingMaterial = thisPartyKeyingMaterial;
            _keyConfirmationFactory = keyConfirmationFactory;
            _macParameters = macParameters;
        }

        public SchemeParametersIfc SchemeParameters { get; }

        public IIfcSecretKeyingMaterial ThisPartyKeyingMaterial { get; }
        public KasResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            var keyToTransport = GetKeyingMaterial(otherPartyKeyingMaterial);

            if (_keyConfirmationFactory == null)
            {
                return new KasResult(keyToTransport, null);
            }

            var keyConfirmationKey = keyToTransport.GetMostSignificantBits(_macParameters.KeyLength);
            var newKeyToTransport = keyToTransport.MSBSubstring(
                _macParameters.KeyLength - 1,keyToTransport.BitLength - _macParameters.KeyLength);
            var keyConfirmationResult = KeyConfirmation(otherPartyKeyingMaterial, keyConfirmationKey);
            
            return new KasResult(newKeyToTransport, keyConfirmationKey, keyConfirmationResult.MacData, keyConfirmationResult.Mac);
        }

        /// <summary>
        /// Creates/Gets/Recovers a key for a KAS/KTS scheme.
        /// </summary>
        /// <param name="otherPartyKeyingMaterial"></param>
        /// <returns></returns>
        protected abstract BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial);

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

        private IKeyConfirmationParameters GetKeyConfirmationParameters(IIfcSecretKeyingMaterial otherPartyKeyingMaterial, BitString keyToTransport)
        {
            var thisPartyEphemData =
                GetEphemeralDataFromKeyContribution(ThisPartyKeyingMaterial, SchemeParameters.KeyAgreementRole);
            var otherPartyEphemData =
                GetEphemeralDataFromKeyContribution(otherPartyKeyingMaterial,
                    KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(SchemeParameters.KeyAgreementRole));
            
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
        /// <returns></returns>
        protected abstract BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial, KeyAgreementRole keyAgreementRole);
    }
}