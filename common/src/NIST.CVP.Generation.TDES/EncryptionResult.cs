using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES
{
    public class EncryptionResult
    {
        public BitString CipherText { get; private set; }
        public string ErrorMessage { get; private set; }
        public EncryptionResult(BitString cipherText)
        {
            CipherText = cipherText;
        }

        public EncryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }
            return $"CipherText: {CipherText.ToHex()}";
        }
    }
}
