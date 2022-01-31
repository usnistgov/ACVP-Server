using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public GeneralCapabilities Capabilities;
        public bool Specific { get; set; } = false;
        public SpecificCapability[] SpecificCapabilities;
    }

    public class GeneralCapabilities
    {
        public string[] LmsTypes { get; set; }
        public string[] LmotsTypes { get; set; }
    }

    public class SpecificCapability
    {
        public LmsLevelParameters[] Levels { get; set; }
    }

    public class LmsLevelParameters
    {
        public string LmsType { get; set; }
        public string LmotsType { get; set; }
    }
}
