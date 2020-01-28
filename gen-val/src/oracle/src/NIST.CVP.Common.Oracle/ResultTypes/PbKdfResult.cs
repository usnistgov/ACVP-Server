using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class PbKdfResult
    {
        public BitString DerivedKey { get; set; }
        public BitString Salt { get; set; }
        public string Password { get; set; }
    }
}