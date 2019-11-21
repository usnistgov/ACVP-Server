using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public string[] Direction { get; set; }
        /// <summary>
        /// Keying Option 1 - 3 independant key TDES
        /// Keying Option 2 - 2 Key TDES
        /// Keying Option 3 (No longer supported) - 1 Key TDES - only used in KATs
        /// </summary>
        public int[] KeyingOption { get; set; }

    }
}
