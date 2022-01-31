using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class SnmpKdfResult
    {
        public string Password { get; set; }
        public BitString SharedKey { get; set; }
    }
}
