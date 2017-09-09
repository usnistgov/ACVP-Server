using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Direction { get; set; }
        public MathDomain MsgLen { get; set; }
        public MathDomain MacLen { get; set; }
    }
}
