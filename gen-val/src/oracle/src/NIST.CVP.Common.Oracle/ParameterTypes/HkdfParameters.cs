using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
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