using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcSignatureResult : IDsaSignatureResult
    {
        public FfcSignature Signature { get; private set; }
        public string ErrorMessage { get; private set; }

        public FfcSignatureResult(FfcSignature signature)
        {
            Signature = signature;
        }

        public FfcSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => !string.IsNullOrEmpty(ErrorMessage);
    }
}
