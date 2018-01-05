using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SRTP
{
    public class SrtpResult
    {
        public BitString AuthenticationKey { get; set; }
        public BitString EncryptionKey { get; set; }
        public BitString SaltingKey { get; set; }

        public SrtpResult(BitString ek, BitString ak, BitString sk)
        {
            AuthenticationKey = ak;
            EncryptionKey = ek;
            SaltingKey = sk;
        }
    }
}
