using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class ShaParameters
    {
        public int MessageLength { get; set; }
        public HashFunction HashFunction { get; set; }
    }
}
