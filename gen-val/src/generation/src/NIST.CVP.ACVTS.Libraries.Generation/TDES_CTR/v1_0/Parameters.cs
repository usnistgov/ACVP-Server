﻿using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        /// <summary>
        /// Keying Option 1 - 3 independant key TDES
        /// Keying Option 2 - 2 Key TDES
        /// Keying Option 3 (No longer supported) - 1 Key TDES - only used in KATs
        /// </summary>
        public int[] KeyingOption { get; set; }
        public string[] Direction { get; set; }

        public MathDomain PayloadLen { get; set; }

        public bool PerformCounterTests { get; set; } = true;
        public bool OverflowCounter { get; set; }
        public bool IncrementalCounter { get; set; }
    }
}
