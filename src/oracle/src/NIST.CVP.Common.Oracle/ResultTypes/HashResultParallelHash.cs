using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class HashResultParallelHash
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public string Customization { get; set; }
        public int BlockSize { get; set; }
        public BitString CustomizationHex { get; set; }
    }
}
