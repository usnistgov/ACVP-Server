using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        /// <summary>
        /// Keying Option 1 - 3 independant key TDES
        /// Keying Option 2 - 2 Key TDES
        /// Keying Option 3 (No longer supported) - 1 Key TDES - only used in KATs
        /// </summary>
        public int[] KeyingOption { get; set; }
        public string[] Direction { get; set; }

        public MathDomain DataLength { get; set; }
        public bool OverflowCounter { get; set; }
        public bool IncrementalCounter { get; set; }
    }
}
