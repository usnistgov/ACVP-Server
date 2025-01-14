using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;

public class MLDSASignatureResult
{
    /// <summary>
    /// Only used in the deferred signature case to verify the signature
    /// </summary>
    public BitString PublicKey { get; set; }  
    
    public BitString Rnd { get; set; }
    public BitString Context { get; set; }
    
    public BitString Mu { get; set; }
    public BitString Message { get; set; }
    public BitString Signature { get; set; }
}
