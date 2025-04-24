using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

public partial interface IOracle
{
    // ML-DSA
    public Task<MLDSAKeyPairResult> GetMLDSAKeyCaseAsync(MLDSAKeyGenParameters param);
    public Task<MLDSASignatureResult> GetMLDSASigGenCaseAsync(MLDSASignatureParameters param);
    public Task<MLDSASignatureResult> GetMLDSASigGenCornerCaseAsync(MLDSASignatureParameters param);
    public Task<MLDSASignatureResult> CompleteMLDSASigGenCornerCaseAsync(MLDSASignatureParameters param, MLDSASignatureResult poolResult);
    public Task<VerifyResult<MLDSASignatureResult>> GetMLDSAVerifyResultAsync(MLDSASignatureParameters param);

    // ML-KEM
    public Task<MLKEMKeyPairResult> GetMLKEMKeyCaseAsync(MLKEMKeyGenParameters param);
    public Task<MLKEMEncapsulationResult> GetMLKEMEncapCaseAsync(MLKEMEncapsulationParameters param);
    public Task<MLKEMEncapsulationResult> GetMLKEMEncapDeferredCaseAsync(MLKEMEncapsulationParameters param);
    public Task<MLKEMEncapsulationResult> GetMLKEMDecapCaseAsync(MLKEMDecapsulationParameters param);   // This function performs encapsulation
    public Task<MLKEMDecapsulationResult> CompleteDeferredMLKEMEncapsulationAsync(MLKEMDecapsulationParameters param, MLKEMEncapsulationResult providedResult);
    
    // SLH-DSA
    public Task<SLHDSAKeyPairResult> GetSLHDSAKeyCaseAsync(SLHDSAKeyGenParameters param);
    public Task<SLHDSASignatureResult> GetSLHDSASigGenCaseAsync(SLHDSASignatureParameters param);
    public Task<VerifyResult<SLHDSASignatureResult>> GetSLHDSASigVerCaseAsync(SLHDSASignatureParameters param);
}
