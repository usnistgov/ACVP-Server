using NIST.CVP.Crypto.Common.Hash.TupleHash;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class TupleHashParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
