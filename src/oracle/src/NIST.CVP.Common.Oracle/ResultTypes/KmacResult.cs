using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KmacResult
    {
        public BitString Message { get; set; }
        public BitString Key { get; set; }
        public BitString Tag { get; set; }
        public BitString CustomizationHex { get; set; }
        public string Customization { get; set; }
        public bool TestPassed { get; set; } = true;
    }
}
