using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class AnsiX942Parameters
    {
        public HashFunction HashAlg { get; set; }
        public int ZzLen { get; set; }
        public int OtherInfoLen { get; set; }
        public int SuppInfoLen { get; set; }
        public int KeyLen { get; set; }
        public BitString Oid { get; set; }
        public AnsiX942Types KdfMode { get; set; }
    }
}
