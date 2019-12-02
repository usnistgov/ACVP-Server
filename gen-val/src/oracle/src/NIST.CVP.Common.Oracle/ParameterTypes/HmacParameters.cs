using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class HmacParameters
    {
        public ModeValues ShaMode { get; set; }

        public DigestSizes ShaDigestSize { get; set; }

        public int KeyLength {get; set; }
        public int MessageLength { get; set; }
        public int MacLength { get; set; }
    }
}