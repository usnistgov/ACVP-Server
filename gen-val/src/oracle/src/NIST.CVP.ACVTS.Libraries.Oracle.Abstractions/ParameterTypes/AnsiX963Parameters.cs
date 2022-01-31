using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class AnsiX963Parameters
    {
        public HashFunction HashAlg { get; set; }
        public int FieldSize { get; set; }
        public int SharedInfoLength { get; set; }
        public int KeyDataLength { get; set; }
    }
}
