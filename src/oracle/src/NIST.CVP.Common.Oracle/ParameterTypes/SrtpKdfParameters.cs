using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class SrtpKdfParameters
    {
        public int AesKeyLength { get; set; }
        public BitString KeyDerivationRate { get; set; }
    }
}
