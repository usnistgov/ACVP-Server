using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class AlgoArrayResponse
    {
        // Can Tuple, Customization, etc. just be added to this file without affecting the other solutions?
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public int DigestLength => Digest.BitLength;
    }
}
