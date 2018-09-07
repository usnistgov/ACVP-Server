using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SRTP
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public int[] AesKeyLength { get; set; }
        public bool SupportsZeroKdr { get; set; }
        public int[] KdrExponent { get; set; }
    }
}
