using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CMAC
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        public string[] Direction { get; set; }

        /// <summary>
        /// AES only
        /// </summary>
        public int[] KeyLen { get; set; } = { };

        /// <summary>
        /// TDES only
        /// </summary>
        public int[] KeyingOption { get; set; } = { };
        public MathDomain MsgLen { get; set; }
        public MathDomain MacLen { get; set; }
    }
}
