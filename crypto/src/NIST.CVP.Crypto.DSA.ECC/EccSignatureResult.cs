using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccSignatureResult : IDsaSignatureResult
    {
        public EccSignature Signature { get; private set; }
        public string ErrorMessage { get; private set; }

        public EccSignatureResult(EccSignature signature)
        {
            Signature = signature;
        }

        public EccSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
