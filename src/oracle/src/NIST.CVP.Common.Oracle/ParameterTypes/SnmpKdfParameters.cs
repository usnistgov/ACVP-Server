using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class SnmpKdfParameters
    {
        public int PasswordLength { get; set; }
        public BitString EngineId { get; set; }
    }
}
