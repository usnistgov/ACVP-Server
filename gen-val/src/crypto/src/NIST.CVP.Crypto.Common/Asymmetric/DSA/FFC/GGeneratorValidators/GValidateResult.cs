namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public class GValidateResult : ICryptoResult
    {
        public string ErrorMessage { get; }

        public GValidateResult() { }

        public GValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
