using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;

public class SLHDSASignatureResult
{
    public BitString AdditionalRandomness { get; set; }
    public int MessageLength { get; set; }
    public BitString Message { get; set; }
    public BitString Signature { get; set; }
    public BitString Context { get; set; }
}
