﻿using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public class PublicKey
    {
        public BigInteger E { get; set; }
        public BigInteger N { get; set; }
    }
}