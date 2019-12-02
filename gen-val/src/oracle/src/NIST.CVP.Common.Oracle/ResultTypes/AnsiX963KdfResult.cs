using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class AnsiX963KdfResult
    {
        public BitString Z { get; set; }
        public BitString SharedInfo { get; set; }
        public BitString KeyOut { get; set; }
    }
}
