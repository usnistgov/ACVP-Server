using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.HKDF.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        public MathDomain SaltLength { get; set; }
        public MathDomain InfoLength { get; set; }
        public MathDomain KeyLength { get; set; }
        public MathDomain InputKeyingMaterialLength { get; set; }
        public string[] HmacAlg { get; set; }
    }
}
