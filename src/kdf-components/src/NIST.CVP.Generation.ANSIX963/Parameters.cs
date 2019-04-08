using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ANSIX963
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public MathDomain SharedInfoLength { get; set; }
        public int[] FieldSize { get; set; }
        public MathDomain KeyDataLength { get; set; }
        public string[] HashAlg { get; set; }
    }
}
