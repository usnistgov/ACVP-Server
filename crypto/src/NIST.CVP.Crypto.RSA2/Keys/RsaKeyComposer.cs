using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class RsaKeyComposer : IRsaKeyComposer
    {
        public KeyPair ComposeKey(BigInteger e, PrimePair primes)
        {
            var n = primes.P * primes.Q;
            var d = e.ModularInverse(NumberTheory.LCM(primes.P - 1, primes.Q - 1));

            return new KeyPair
            {
                PrivKey = new PrivateKey
                {
                    D = d,
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
