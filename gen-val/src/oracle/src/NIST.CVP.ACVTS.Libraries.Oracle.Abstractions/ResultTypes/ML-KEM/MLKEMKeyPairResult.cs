using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

public class MLKEMKeyPairResult
{
    public BitString SeedZ { get; set; }
    public BitString SeedD { get; set; }
    public BitString EncapsulationKey { get; set; }
    public BitString DecapsulationKey { get; set; }
}
