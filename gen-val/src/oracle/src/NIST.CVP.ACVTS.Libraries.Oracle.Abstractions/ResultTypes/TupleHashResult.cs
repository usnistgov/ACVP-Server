using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class TupleHashResult : IResult
    {
        public List<BitString> Tuple { get; set; }
        public BitString Digest { get; set; }
        public string Customization { get; set; }
        public string FunctionName { get; set; }
        public BitString CustomizationHex { get; set; }
    }
}
