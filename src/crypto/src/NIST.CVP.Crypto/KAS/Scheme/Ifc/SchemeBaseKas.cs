using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    internal abstract class SchemeBaseKas : SchemeBase
    {
        protected IKdfVisitor _kdfVisitor;
        protected IKdfParameter _kdfParameter;
        protected IRsaSve _rsaSve;


        protected SchemeBaseKas
        (
            SchemeParametersIfc schemeParameters, 
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve
            ) : base(schemeParameters, fixedInfoFactory, fixedInfoParameter, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters)
        {
            _kdfVisitor = kdfVisitor;
            _kdfParameter = kdfParameter;
            _rsaSve = rsaSve;
        }
    }
}