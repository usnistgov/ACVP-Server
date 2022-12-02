using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ifc
{
    internal abstract class SchemeBaseKas : SchemeBase
    {
        protected IKdfVisitor _kdfVisitor;
        protected IKdfParameter _kdfParameter;
        protected IRsaSve _rsaSve;

        protected SchemeBaseKas
        (
            IEntropyProvider entropyProvider,
            SchemeParametersIfc schemeParameters,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve
            ) : base(entropyProvider, schemeParameters, fixedInfoFactory, fixedInfoParameter, thisPartyKeyingMaterialBuilder, keyConfirmationFactory, macParameters)
        {
            _kdfVisitor = kdfVisitor;
            _kdfParameter = kdfParameter;
            _rsaSve = rsaSve;
        }
    }
}
