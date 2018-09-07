using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public string[] Curve { get; set; }
        public string[] SecretGenerationMode { get; set; }
    }
}
