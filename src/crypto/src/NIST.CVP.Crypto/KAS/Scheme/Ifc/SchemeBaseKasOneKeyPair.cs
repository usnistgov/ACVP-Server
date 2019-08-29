using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public class SchemeBaseKasOneKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasOneKeyPair
        (
            SchemeParametersIfc schemeParameters, 
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfFactory kdfFactory,
            IRsaSve rsaSve
            ) 
            : base(schemeParameters, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters, kdfFactory, rsaSve)
        {
        }

        protected override BitString GetKeyToTransport(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                var otherPartyPublicKey = otherPartyKeyingMaterial.Key.PubKey;

                var z = ThisPartyKeyingMaterial.Z;
                
                
            }
            
            throw new System.NotImplementedException();
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole)
        {
            // KAS1        Party U        C
            // KAS1        Party V        NV

            if (keyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return secretKeyingMaterial.C;
            }

            return secretKeyingMaterial.DkmNonce;
        }

        protected override KdfResult GetKeyFromPartyContributions()
        {
            throw new System.NotImplementedException();
        }
    }
}