namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IDsaFfc : 
        IDsa<
            FfcDomainParametersGenerateRequest, 
            FfcDomainParametersGenerateResult, 
            FfcDomainParameters, 
            FfcKeyPairGenerateResult,
            FfcKeyPair, 
            FfcKeyPairValidateResult
        >
    {
    }
}