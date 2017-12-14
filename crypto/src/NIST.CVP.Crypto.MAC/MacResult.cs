using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.MAC
{
    public class MacResult
    {
        public BitString Mac { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public MacResult(BitString mac)
        {
            Mac = mac;
        }

        public MacResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
