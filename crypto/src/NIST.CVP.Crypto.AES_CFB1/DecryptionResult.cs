namespace NIST.CVP.Crypto.AES_CFB1
{
    public class DecryptionResult
    {
        public BitOrientedBitString PlainText { get; private set; }
        public string ErrorMessage { get; private set; }
        public DecryptionResult(BitOrientedBitString plainText)
        {
            PlainText = plainText;
        }

        public DecryptionResult(string errorMessage)
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

            return $"PlainText: {PlainText.ToHex()}";
        }
    }
}