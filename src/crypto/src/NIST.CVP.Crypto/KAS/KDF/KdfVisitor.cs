using System;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfVisitor : IKdfVisitor
    {
        private readonly IKdfOneStepFactory _kdfOneStepFactory;

        public KdfVisitor(IKdfOneStepFactory kdfOneStepFactory)
        {
            _kdfOneStepFactory = kdfOneStepFactory;
        }

        public KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo)
        {
            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction);

            return kdf.DeriveKey(param.Z, param.L, fixedInfo, param.Salt);
        }
    }
}