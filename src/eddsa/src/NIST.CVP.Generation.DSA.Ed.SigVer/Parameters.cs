using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.SigVer
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public bool Pure { get; set; } = true;
        public bool PreHash { get; set; } = false;
        public string[] Curve { get; set; }
    }
}
