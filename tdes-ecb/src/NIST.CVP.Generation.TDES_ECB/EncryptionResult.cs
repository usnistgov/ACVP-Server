using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
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
            return $"Cipher: {CipherText.ToHex()}";
        }
    }
}
