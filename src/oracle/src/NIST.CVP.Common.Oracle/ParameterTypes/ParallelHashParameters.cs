using NIST.CVP.Crypto.Common.Hash.ParallelHash;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class ParallelHashParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
