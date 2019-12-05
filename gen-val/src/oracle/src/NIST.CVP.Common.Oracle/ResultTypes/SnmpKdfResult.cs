using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class SnmpKdfResult
    {
        public string Password { get; set; }
        public BitString SharedKey { get; set; }
    }
}
