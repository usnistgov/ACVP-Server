using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class Parameters : IParameters
    {
        public string[] Curves { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
    }
}