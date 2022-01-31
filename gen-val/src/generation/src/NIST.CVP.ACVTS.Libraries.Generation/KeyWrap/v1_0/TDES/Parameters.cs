using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES
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
        public string[] KwCipher { get; set; }
        public MathDomain PayloadLen { get; set; }
    }
}
