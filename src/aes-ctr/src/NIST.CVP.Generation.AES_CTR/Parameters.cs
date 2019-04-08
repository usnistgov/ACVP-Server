using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CTR
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public int[] KeyLen { get; set; }
        public string[] Direction { get; set; }

        public MathDomain PayloadLen { get; set; }
        public bool OverflowCounter { get; set; }
        public bool IncrementalCounter { get; set; }
    }
}
