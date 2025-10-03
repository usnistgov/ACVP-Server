using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;

public class MLDSASignatureParameters : IParameters
{
    public DilithiumParameterSet ParameterSet { get; set; }
    public bool Deterministic { get; set; }
    public int MessageLength { get; set; }
    public BitString PrivateKey { get; set; }
    public SignatureInterface SignatureInterface { get; set; }
    public int ContextLength { get; set; }
    public bool ExternalMu { get; set; }
    
    public PreHash PreHash { get; set; }
    
    /// <summary>
    /// Only used for PreHash
    /// </summary>
    public HashFunctions HashFunction { get; set; }
    
    /// <summary>
    /// Used only in SigVer (MLDSAVerifyCaseGrain)
    /// </summary>
    public MLDSASignatureDisposition Disposition { get; set; }
    
    /// <summary>
    /// Corner cases provided by the Pools
    /// </summary>
    public MLDSASignatureCornerCase CornerCase { get; set; }
    
    public override bool Equals(object other)
    {
        if (other is MLDSASignatureParameters p)
        {
            return GetHashCode() == p.GetHashCode();
        }

        return false;
    }
    
    // [Potential TODO] would need to be updated if other properties are included in the pools
    public override int GetHashCode() => HashCode.Combine(ParameterSet, CornerCase);
}
