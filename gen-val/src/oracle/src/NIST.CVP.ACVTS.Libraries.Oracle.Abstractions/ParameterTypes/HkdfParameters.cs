using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class HkdfParameters
    {
        public HashFunction HmacAlg { get; set; }
        public int SaltLen { get; set; }
        public int InputLen { get; set; }
        public int KeyLen { get; set; }
        public int InfoLen { get; set; }
    }
}
