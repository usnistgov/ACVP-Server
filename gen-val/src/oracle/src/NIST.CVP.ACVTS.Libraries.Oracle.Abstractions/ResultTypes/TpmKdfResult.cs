using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class TpmKdfResult
    {
        public BitString Auth { get; set; }
        public BitString NonceEven { get; set; }
        public BitString NonceOdd { get; set; }
        public BitString SKey { get; set; }
    }
}
