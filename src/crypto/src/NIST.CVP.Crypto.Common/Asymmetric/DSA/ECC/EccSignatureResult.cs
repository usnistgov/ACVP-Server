﻿namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccSignatureResult : IDsaSignatureResult
    {
        public EccSignature Signature { get; }
        public string ErrorMessage { get; }

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
