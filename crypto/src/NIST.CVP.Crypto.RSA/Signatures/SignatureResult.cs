using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class SignatureResult
    {
        public BitString Signature { get; private set; }
        public string ErrorMessage { get; private set; }

        public SignatureResult(BitString signature)
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
