using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CCM
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public int[] KeyLen { get; set; }
        public MathDomain PtLen { get; set; }
        public MathDomain IvLen { get; set; }
        public MathDomain AadLen { get; set; }
        public MathDomain TagLen { get; set; }
        public bool SupportsAad2Pow16 { get; set; }
    }
}
