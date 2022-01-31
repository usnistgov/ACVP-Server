using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class AnsiX963KdfResult
    {
        public BitString Z { get; set; }
        public BitString SharedInfo { get; set; }
        public BitString KeyOut { get; set; }
    }
}
