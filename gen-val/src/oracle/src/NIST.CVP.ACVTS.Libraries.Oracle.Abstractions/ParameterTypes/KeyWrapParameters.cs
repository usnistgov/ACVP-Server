using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class KeyWrapParameters
    {
        public KeyWrapType KeyWrapType { get; set; }
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }
        public bool WithInverseCipher { get; set; }
        public bool CouldFail { get; set; }
    }
}
