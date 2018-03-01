using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class AlgoArrayResponseSignature
    {
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        public KeyPair Key { get; set; }
        public bool FailureTest { get; set; }
    }
}
