using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_GCM
{
    public class EncryptionResult
    {
        public BitString CipherText { get; private set; }
        public BitString Tag { get; private set; }
        public string ErrorMessage { get; private set; }
        public EncryptionResult(BitString cipherText, BitString tag)
        {
            CipherText = cipherText;
            Tag = tag;
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
