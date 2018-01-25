using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class CrtKeyComposer : IRsaKeyComposer
    {
        public KeyPair ComposeKey(BigInteger e, PrimePair primes)
        {
            var n = primes.P * primes.Q;
            var d = e.ModularInverse(n);

            return new KeyPair
            {
                PrivKey = new CrtPrivateKey
                {
                    DMP1 = d % (primes.P - 1),
                    DMQ1 = d % (primes.Q - 1),
                    IQMP = primes.Q.ModularInverse(primes.P),
                    P = primes.P,
                    Q = primes.Q
                },
                PubKey = new PublicKey
                {
                    E = e,
                    N = n
                }
            };
        }
    }
}
