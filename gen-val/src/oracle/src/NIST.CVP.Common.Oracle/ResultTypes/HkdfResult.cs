using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class HkdfResult
    {
        public BitString OtherInfo { get; set; }
        public BitString InputKeyingMaterial { get; set; }
        public BitString Salt { get; set; }
        public BitString DerivedKey { get; set; }
    }
}