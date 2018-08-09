using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AesParameters : IParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }
    }
}
