using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;

public class SLHDSASignatureParameters
{
    public SlhdsaParameterSet SlhdsaParameterSet { get; set; }
    public bool Deterministic { get; set; }
    public int MessageLength { get; set; }
    public PreHash PreHash { get; set; }
    public SignatureInterface SignatureInterface { get; set; }
    public BitString PrivateKey { get; set; }
    public HashFunctions HashFunction { get; set; }
    public int ContextLength { get; set; }
    
    /// <summary>
    /// Used only for SigVer (SLHDSAVerifyCaseGrain)
    /// </summary>
    public SLHDSASignatureDisposition Disposition { get; set; }
}
