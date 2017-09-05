namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IDsaFfc : 
        IDsa<
            FfcDomainParametersGenerateRequest, 
            FfcDomainParametersGenerateResult, 
            FfcDomainParametersValidateRequest,
            FfcDomainParametersValidateResult,
            FfcDomainParameters, 
            FfcKeyPairGenerateResult,
            FfcKeyPair, 
            FfcKeyPairValidateResult
        >
    {
    }
}