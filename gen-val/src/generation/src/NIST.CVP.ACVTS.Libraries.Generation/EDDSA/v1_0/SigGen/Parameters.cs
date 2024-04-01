using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public bool Pure { get; set; } = true;
        public bool PreHash { get; set; } = false;
        public string[] Curve { get; set; }
        public MathDomain ContextLength { get; set; } = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255, 1));
    }
}
