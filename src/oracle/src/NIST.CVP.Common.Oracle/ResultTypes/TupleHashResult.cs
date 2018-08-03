using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class TupleHashResult
    {
        public List<BitString> Tuple { get; set; }
        public BitString Digest { get; set; }
        public string Customization { get; set; }
        public BitString CustomizationHex { get; set; }
    }
}
