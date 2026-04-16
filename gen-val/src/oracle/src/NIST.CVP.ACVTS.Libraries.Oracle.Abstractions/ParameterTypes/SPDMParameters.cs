using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

public class SPDMParameters
{
    public int KeyLength { get; set; }
    public int THLength { get; set; }
    public HashFunctions Mode { get; set; }
    public SPDMVersions Version { get; set; }
    public bool PSK { get; set; }
}
