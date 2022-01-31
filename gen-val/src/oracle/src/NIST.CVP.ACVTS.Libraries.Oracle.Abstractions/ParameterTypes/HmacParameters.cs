using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class HmacParameters
    {
        public ModeValues ShaMode { get; set; }

        public DigestSizes ShaDigestSize { get; set; }

        public int KeyLength { get; set; }
        public int MessageLength { get; set; }
        public int MacLength { get; set; }
    }
}
