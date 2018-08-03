using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TdesParameters
    {
        public BlockCipherModesOfOperation Mode { get; set; }
        public int KeyingOption { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }
    }
}