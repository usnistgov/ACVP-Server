namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators
{
    public class PQValidateResult
    {
        public string ErrorMessage { get; }

        public PQValidateResult() { }

        public PQValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
