namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcDomainParametersValidateResult : IDomainParametersValidateResult
    {
        public string ErrorMessage { get; }

        public FfcDomainParametersValidateResult() { }

        public FfcDomainParametersValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}