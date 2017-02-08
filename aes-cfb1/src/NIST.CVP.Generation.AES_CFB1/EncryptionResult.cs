using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class EncryptionResult
    {
        public BitOrientedBitString CipherText { get; private set; }
        public string ErrorMessage { get; private set; }
        public EncryptionResult(BitOrientedBitString cipherText)
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
            return $"Cipher: {CipherText.ToHex()}";
        }
    }
}