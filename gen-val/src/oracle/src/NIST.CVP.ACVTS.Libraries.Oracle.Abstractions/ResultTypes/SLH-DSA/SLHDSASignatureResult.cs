using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;

public class SLHDSASignatureResult
{
    /// <summary>
    /// Only used for signature verification
    /// </summary>
    public BitString PublicKey { get; set; }
    public BitString PrivateKey { get; set; }
    public BitString AdditionalRandomness { get; set; }
    public int MessageLength { get; set; }
    public BitString Message { get; set; }
    public BitString Signature { get; set; }
}
