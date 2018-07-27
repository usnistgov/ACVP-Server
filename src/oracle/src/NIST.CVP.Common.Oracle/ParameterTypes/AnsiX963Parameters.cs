using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AnsiX963Parameters
    {
        public HashFunction HashAlg { get; set; }
        public int FieldSize { get; set; }
        public int SharedInfoLength { get; set; }
        public int KeyDataLength { get; set; }
    }
}
