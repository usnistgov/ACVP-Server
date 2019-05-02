using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.PBKDF.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
        
        public MathDomain IterationCount { get; set; }
        public MathDomain PasswordLength { get; set; }
        public MathDomain SaltLength { get; set; }
        public MathDomain KeyLength { get; set; }
    }
}