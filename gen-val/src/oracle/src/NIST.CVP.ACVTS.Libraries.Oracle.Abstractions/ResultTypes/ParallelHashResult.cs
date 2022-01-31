using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class ParallelHashResult : IResult
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
        public string Customization { get; set; }
        public int BlockSize { get; set; }
        public BitString CustomizationHex { get; set; }
        public string FunctionName { get; set; }
    }
}
