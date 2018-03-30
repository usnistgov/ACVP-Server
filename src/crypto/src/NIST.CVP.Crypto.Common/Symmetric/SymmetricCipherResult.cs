using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCipherResult : ICryptoResult
    {
        public BitString Result { get; }
        public string ErrorMessage { get; }
        public SymmetricCipherResult(BitString result)
        {
            Result = result;
        }

        public SymmetricCipherResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }
            return $"Result: {Result.ToHex()}";
        }
    }
}