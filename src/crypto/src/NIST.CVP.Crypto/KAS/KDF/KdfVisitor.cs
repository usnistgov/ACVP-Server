using System;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfVisitor : IKdfVisitor
    {
        private readonly IKdfOneStepFactory _kdfOneStepFactory;

        public KdfVisitor(IKdfOneStepFactory kdfOneStepFactory)
        {
            _kdfOneStepFactory = kdfOneStepFactory;
        }

        public KdfResult Kdf(KdfParameterOneStep param)
        {
            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction.AuxFunctionName);

            return kdf.DeriveKey(param.Z, param.L, param.FixedInfo, param.Salt);
        }
    }
}