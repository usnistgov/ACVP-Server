﻿namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public class VerifyResult : ICryptoResult
    {
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public VerifyResult() { }

        public VerifyResult(string error)
        {
            ErrorMessage = error;
        }
    }
}