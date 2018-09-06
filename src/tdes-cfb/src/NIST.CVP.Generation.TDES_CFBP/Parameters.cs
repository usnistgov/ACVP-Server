using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public string[] Direction { get; set; }
        public int[] KeyingOption { get; set; }
    }
}
