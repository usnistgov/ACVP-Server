using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCipherAeadResult : ICryptoResult
    {
        public BitString CipherText { get; }
        public BitString Tag { get; }
        public string ErrorMessage { get; }
        public SymmetricCipherAeadResult(BitString cipherText, BitString tag)
        {
            CipherText = cipherText;
            Tag = tag;
        }

        public SymmetricCipherAeadResult(string errorMessage)
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
            return $"Cipher: {CipherText.ToHex()}";
        }
    }
}
