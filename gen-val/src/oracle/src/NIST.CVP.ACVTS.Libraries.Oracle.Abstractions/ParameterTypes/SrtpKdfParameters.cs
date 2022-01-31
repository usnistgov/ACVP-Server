using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class SrtpKdfParameters
    {
        public int AesKeyLength { get; set; }
        public BitString KeyDerivationRate { get; set; }
    }
}
