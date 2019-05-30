using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Common.Oracle.ResultTypes
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
