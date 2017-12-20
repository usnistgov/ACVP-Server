using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.IKEv1
{
    public class IkeResult
    {
        public BitString SKeyId { get; }
        public BitString SKeyIdA { get; }
        public BitString SKeyIdD { get; }
        public BitString SKeyIdE { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public IkeResult(BitString id, BitString idA, BitString idD, BitString idE)
        {
            SKeyId = id;
            SKeyIdA = idA;
            SKeyIdD = idD;
            SKeyIdE = idE;
        }

        public IkeResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
