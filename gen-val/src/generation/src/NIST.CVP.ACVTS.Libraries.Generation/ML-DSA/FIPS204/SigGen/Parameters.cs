using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class Parameters : PqcParametersBase, IParameters
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; }
    public string Mode { get; set; }
    public string Revision { get; set; }
    public bool IsSample { get; set; }
    public string[] Conformances { get; set; }

    // signatureInterfaces, preHash inherited from base class
    
    public Capability[] Capabilities = Array.Empty<Capability>();
    public bool[] Deterministic { get; set; }
    public bool[] ExternalMu { get; set; } = { false }; // Default to false
}

public class Capability : PqcCapabilityBase
{
    // MessageLength, HashAlgs, ContextLength inherited from base class

    public DilithiumParameterSet[] ParameterSets { get; set; } = Array.Empty<DilithiumParameterSet>();
}
