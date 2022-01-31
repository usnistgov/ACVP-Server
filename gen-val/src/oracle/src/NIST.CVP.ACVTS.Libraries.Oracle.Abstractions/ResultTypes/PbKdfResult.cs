using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class PbKdfResult
    {
        public BitString DerivedKey { get; set; }
        public BitString Salt { get; set; }
        public string Password { get; set; }
    }
}
