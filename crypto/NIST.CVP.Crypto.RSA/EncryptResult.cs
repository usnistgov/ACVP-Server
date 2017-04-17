using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class EncryptResult
    {
        public BigInteger Ciphertext { get; private set; }
        public string ErrorMessage { get; private set; }

        public EncryptResult(BigInteger ciphertext)
        {
            Ciphertext = ciphertext;
        }

        public EncryptResult(string errorMessage)
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

            return $"Ciphertext: {Ciphertext}";
        }
    }
}
