using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public abstract class SchemeBaseKas : SchemeBase
    {
        private IKdfVisitor _kdfVisitor;
        private IKdfParameter _kdfParameter;
        private IRsaSve _rsaSve;


        protected SchemeBaseKas
        (
            SchemeParametersIfc schemeParameters, 
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve
            ) : base(schemeParameters, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters)
        {
            _kdfVisitor = kdfVisitor;
            _kdfParameter = kdfParameter;
            _rsaSve = rsaSve;
        }

        protected abstract KdfResult Kdf(IIfcSecretKeyingMaterial otherPartySecretKeyingMaterial);
    }
}