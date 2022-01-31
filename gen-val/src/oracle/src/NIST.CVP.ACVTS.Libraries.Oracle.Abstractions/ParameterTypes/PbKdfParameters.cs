using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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
