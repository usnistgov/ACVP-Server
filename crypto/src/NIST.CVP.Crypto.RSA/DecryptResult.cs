using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class DecryptResult
    {
        public BigInteger Plaintext { get; private set; }
        public string ErrorMessage { get; private set; }

        public DecryptResult(BigInteger plaintext)
        {
            Plaintext = plaintext;
        }

        public DecryptResult(string errorMessage)
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

            return $"Plaintext: {Plaintext}";
        }
    }
}
