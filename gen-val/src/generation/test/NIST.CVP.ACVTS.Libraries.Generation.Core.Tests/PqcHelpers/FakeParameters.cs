using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.PqcHelpers;

public class FakeParameters : PqcParametersBase, IParameters
{
    public int VectorSetId { get; init; }
    public string Algorithm { get; init; }
    public string Mode { get; init; }
    public string Revision { get; init; }
    public bool IsSample { get; init; }
    public string[] Conformances { get; init; }

    public FakeCapability[] Capabilities { get; set; } = Array.Empty<FakeCapability>();
}

public class FakeCapability : PqcCapabilityBase
{
    
}
