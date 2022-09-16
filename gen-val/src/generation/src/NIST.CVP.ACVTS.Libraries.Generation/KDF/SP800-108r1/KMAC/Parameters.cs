using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
        
        public MacModes[] MacMode { get; set; }
        public MathDomain KeyDerivationKeyLength { get; set; }
        public MathDomain ContextLength { get; set; }
        public MathDomain LabelLength { get; set; } = null;
        public MathDomain DerivedKeyLength { get; set; }
    }
}
