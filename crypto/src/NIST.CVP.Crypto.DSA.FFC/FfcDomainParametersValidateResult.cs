namespace NIST.CVP.Crypto.DSA.FFC
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