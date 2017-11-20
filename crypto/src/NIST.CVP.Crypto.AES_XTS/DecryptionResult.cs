using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public class DecryptionResult
    {
        public BitString PlainText { get; }
        public string ErrorMessage { get; }

        public DecryptionResult(BitString plaintext)
        {
            PlainText = plaintext;
        }

        public DecryptionResult(string errorMessage)
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

            return $"PlainText: {PlainText.ToHex()}";
        }
    }
}
