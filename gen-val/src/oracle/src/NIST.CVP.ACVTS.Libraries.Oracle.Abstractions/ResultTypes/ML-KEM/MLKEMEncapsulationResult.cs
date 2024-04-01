using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

public class MLKEMEncapsulationResult
{
    public BitString SeedM { get; set; }
    public BitString SharedKey { get; set; }
    public BitString Ciphertext { get; set; }
}
