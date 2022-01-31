using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class HkdfResult
    {
        public BitString OtherInfo { get; set; }
        public BitString InputKeyingMaterial { get; set; }
        public BitString Salt { get; set; }
        public BitString DerivedKey { get; set; }
    }
}
