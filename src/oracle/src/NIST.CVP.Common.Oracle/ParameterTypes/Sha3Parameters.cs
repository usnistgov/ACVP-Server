using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class Sha3Parameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
