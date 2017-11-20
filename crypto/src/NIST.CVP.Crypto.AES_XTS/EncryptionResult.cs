using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.AES_XTS
{
    public class EncryptionResult
    {
        public BitString CipherText { get; }
        public string ErrorMessage { get; }

        public EncryptionResult(BitString ciphertext)
        {
            CipherText = ciphertext;
        }

        public EncryptionResult(string errorMessage)
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

            return $"CipherText: {CipherText.ToHex()}";
        }
    }
}
