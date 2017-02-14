using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class DecryptionResult
    {
        public BitString PlainText { get; private set; }
        public string ErrorMessage { get; private set; }
        public DecryptionResult(BitString plainText)
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