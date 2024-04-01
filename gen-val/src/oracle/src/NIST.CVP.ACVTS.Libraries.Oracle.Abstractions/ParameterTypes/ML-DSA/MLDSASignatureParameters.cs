using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;

public class MLDSASignatureParameters
{
    public DilithiumParameterSet ParameterSet { get; set; }
    public bool Deterministic { get; set; }
    public int MessageLength { get; set; }
    public BitString PrivateKey { get; set; }
    
    /// <summary>
    /// Used only in SigVer (MLDSAVerifyCaseGrain)
    /// </summary>
    public MLDSASignatureDisposition Disposition { get; set; }
}
