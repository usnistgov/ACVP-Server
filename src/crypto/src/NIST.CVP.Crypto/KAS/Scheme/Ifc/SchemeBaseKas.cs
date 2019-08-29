using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public abstract class SchemeBaseKas : SchemeBase
    {
        private IRsaSve _rsaSve;
        private IKdfFactory _kdfFactory;

        protected SchemeBaseKas
        (
            SchemeParametersIfc schemeParameters, 
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfFactory kdfFactory,
            IRsaSve rsaSve
            ) : base(schemeParameters, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters)
        {
            _kdfFactory = kdfFactory;
            _rsaSve = rsaSve;
        }

        protected abstract KdfResult GetKeyFromPartyContributions();
    }
}