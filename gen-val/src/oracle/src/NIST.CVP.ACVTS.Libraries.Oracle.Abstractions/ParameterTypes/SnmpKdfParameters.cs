using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class SnmpKdfParameters
    {
        public int PasswordLength { get; set; }
        public BitString EngineId { get; set; }
    }
}
