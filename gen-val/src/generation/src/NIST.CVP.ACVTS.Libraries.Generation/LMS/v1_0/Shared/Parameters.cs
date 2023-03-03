using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;

public class Parameters : IParameters
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; }
    public string Mode { get; set; }
    public string Revision { get; set; }
    public bool IsSample { get; set; }
    public string[] Conformances { get; set; } = { };

    public GeneralCapabilities Capabilities;
    public SpecificCapability[] SpecificCapabilities;
}

public class GeneralCapabilities
{
    public LmsMode[] LmsModes { get; set; }
    public LmOtsMode[] LmOtsModes { get; set; }
}

public class SpecificCapability
{
    public LmsMode LmsMode { get; set; }
    public LmOtsMode LmOtsMode { get; set; }
}
