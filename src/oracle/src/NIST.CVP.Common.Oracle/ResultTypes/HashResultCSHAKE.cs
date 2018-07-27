using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class HashResultCSHAKE
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public string Customization { get; set; }
        public string FunctionName { get; set; }
        public BitString CustomizationHex { get; set; }
    }
}
