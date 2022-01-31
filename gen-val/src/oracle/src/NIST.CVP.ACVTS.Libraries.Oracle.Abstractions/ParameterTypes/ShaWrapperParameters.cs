using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class ShaWrapperParameters
    {
        public HashFunction HashFunction { get; set; }
        public int MessageLength { get; set; }
    }
}
