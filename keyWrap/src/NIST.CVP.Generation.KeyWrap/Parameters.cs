using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public int[] KeyLen { get; set; }
        public string[] Direction { get; set; }
        public string[] KwCipher { get; set; }
        public MathDomain PtLen { get; set; }
        public int[] KeyingOption { get; set; } 
    }
}
