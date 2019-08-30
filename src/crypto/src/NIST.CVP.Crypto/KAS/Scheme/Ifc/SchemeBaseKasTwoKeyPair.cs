using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public class SchemeBaseKasTwoKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasTwoKeyPair
        (
            SchemeParametersIfc schemeParameters, 
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve) 
            : base(schemeParameters, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters, kdfVisitor, kdfParameter, rsaSve)
        {
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            throw new System.NotImplementedException();
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole)
        {
            return secretKeyingMaterial.C;
        }

        protected override KdfResult Kdf(IIfcSecretKeyingMaterial otherPartySecretKeyingMaterial)
        {
            throw new System.NotImplementedException();
        }
    }
}