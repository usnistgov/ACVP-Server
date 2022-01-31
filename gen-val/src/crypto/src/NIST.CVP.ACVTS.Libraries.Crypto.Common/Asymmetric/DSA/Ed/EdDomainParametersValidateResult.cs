namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdDomainParametersValidateResult : IDomainParametersValidateResult
    {
        public string ErrorMessage { get; }

        public EdDomainParametersValidateResult() { }

        public EdDomainParametersValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
