using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TPMv1._2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
    }
}
