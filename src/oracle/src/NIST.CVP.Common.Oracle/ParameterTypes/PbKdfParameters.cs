using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class PbKdfParameters
    {
        public HashFunction HashAlg { get; set; }
        public int KeyLen { get; set; }
        public int SaltLen { get; set; }
        public int PassLen { get; set; }
        public int ItrCount { get; set; }
    }
}