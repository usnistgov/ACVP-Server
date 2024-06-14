using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;

public class SLHDSAKeyPairResult
{
    public BitString SKSeed { get; set; }
    
    public BitString SKPrf { get; set; }
    
    public BitString PKSeed { get; set; }
    
    public BitString PrivateKey { get; set; }
    
    public BitString PublicKey { get; set; }
}
