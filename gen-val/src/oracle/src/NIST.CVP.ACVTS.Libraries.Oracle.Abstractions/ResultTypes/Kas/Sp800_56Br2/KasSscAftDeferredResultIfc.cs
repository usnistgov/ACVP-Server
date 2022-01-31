using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2
{
    public class KasSscAftDeferredResultIfc : ICryptoResult
    {
        public IIfcSecretKeyingMaterial ServerKeyingMaterial { get; set; }
        public IIfcSecretKeyingMaterial IutKeyingMaterial { get; set; }
        public KasResult Result { get; set; }
        public BitString HashZ { get; set; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
    }
}
