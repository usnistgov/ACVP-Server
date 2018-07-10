using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Oracle.KAS.Ecc
{
    public class KasValEccTestGeneratorFactory : IKasValTestGeneratorFactory<KasValParametersEcc, KasValResultEcc>
    {
        public IKasValTestGenerator<KasValParametersEcc, KasValResultEcc> GetInstance(KasMode kasMode)
        {
            switch (kasMode)
            {
                case KasMode.NoKdfNoKc:
                    return new KasValFfcTestGeneratorNoKdfNoKc();
                case KasMode.KdfNoKc:
                    return new KasValEccTestGeneratorKdfNoKc();
                case KasMode.KdfKc:
                    return new KasValEccTestGeneratorKdfKc();
                default:
                    throw new ArgumentException(nameof(kasMode));
            }
        }
    }
}
