using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public string[] Conformances { get; set; } = { };
        public string[] Direction { get; set; }
        public int[] KeyLen { get; set; }
        public MathDomain PayloadLen { get; set; }
        public MathDomain AadLen { get; set; }
    }
}
