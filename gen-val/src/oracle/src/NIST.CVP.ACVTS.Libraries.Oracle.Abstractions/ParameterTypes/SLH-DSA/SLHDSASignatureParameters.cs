using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;

public class SLHDSASignatureParameters
{
    public SlhdsaParameterSet SlhdsaParameterSet { get; set; }
    public bool Deterministic { get; set; }
    public int MessageLength { get; set; }
    
    /// <summary>
    /// Used only for SigVer (SLHDSAVerifyCaseGrain)
    /// </summary>
    public SLHDSASignatureDisposition Disposition { get; set; }
}
