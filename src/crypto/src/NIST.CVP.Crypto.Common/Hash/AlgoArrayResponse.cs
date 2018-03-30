using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class AlgoArrayResponse
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
