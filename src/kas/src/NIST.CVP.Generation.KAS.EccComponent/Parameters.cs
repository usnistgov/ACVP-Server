using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string KasMode { get; set; }
        public bool IsSample { get; set; }
        public string[] Function { get; set; }
        public string[] Curve { get; set; }
    }
}