namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IDsaEcc : IDsa<
        EccDomainParametersGenerateRequest,
        EccDomainParametersGenerateResult,
        EccDomainParmaeters,
        EccKeyPair,
        EccKeyPairValidationResult
        >
    {
    }
}