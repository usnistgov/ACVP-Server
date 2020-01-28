using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class TpmKdfResult
    {
        public BitString Auth { get; set; }
        public BitString NonceEven { get; set; }
        public BitString NonceOdd { get; set; }
        public BitString SKey { get; set; }
    }
}
