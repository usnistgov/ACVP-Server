using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class CmacParameters
    {
        public CmacTypes CmacType { get; set; }
        public bool CouldFail { get; set; }
        public int PayloadLength { get; set; }
        public int KeyLength { get; set; }
        public int KeyingOption {get; set; }
        public int MacLength { get; set; }
    }
}