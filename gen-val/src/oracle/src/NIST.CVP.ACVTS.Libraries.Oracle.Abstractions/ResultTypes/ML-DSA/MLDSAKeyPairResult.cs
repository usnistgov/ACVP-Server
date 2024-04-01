using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;

public class MLDSAKeyPairResult
{
    public BitString PublicKey { get; set; }
    public BitString PrivateKey { get; set; }
    public BitString Seed { get; set; }
}
