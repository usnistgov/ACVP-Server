using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class SignatureResult
    {
        public BigInteger Signature { get; private set; }
        public string ErrorMessage { get; private set; }

        public SignatureResult(BigInteger signature)
        {
            Signature = signature;
        }

        public SignatureResult(string errorMessage)
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

            return $"Signature: {Signature}";
        }
    }
}
