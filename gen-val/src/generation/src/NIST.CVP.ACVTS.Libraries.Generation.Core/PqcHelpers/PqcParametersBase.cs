using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

public abstract class PqcParametersBase
{
    public SignatureInterface[] SignatureInterfaces { get; set; } = Array.Empty<SignatureInterface>();
    public PreHash[] PreHash { get; set; } = Array.Empty<PreHash>(); 
}

public abstract class PqcCapabilityBase
{
    public MathDomain MessageLength { get; set; }
    public HashFunctions[] HashAlgs { get; set; } = Array.Empty<HashFunctions>();
    public MathDomain ContextLength { get; set; }
}
