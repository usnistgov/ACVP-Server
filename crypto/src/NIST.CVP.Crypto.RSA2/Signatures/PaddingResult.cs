using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class PaddingResult
    {
        public BitString PaddedMessage { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PaddingResult(BitString paddedMessage)
        {
            PaddedMessage = paddedMessage;
        }

        public PaddingResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
