using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math.Helpers;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Keys
{
    public class CrtKeyComposer : IRsaKeyComposer
    {
        public KeyPair ComposeKey(BigInteger e, PrimePair primes)
        {
            var n = primes.P * primes.Q;
            
            // Checks to avoid exceptions
            if (primes.P == 0 || primes.Q == 0 || e == 0)
            {
                return new KeyPair
                {
                    PrivKey = new CrtPrivateKey {P = primes.P, Q = primes.Q},
                    PubKey = new PublicKey {E = e, N = n}
                };
            }
            
            var d = e.ModularInverse(NumberTheory.LCM(primes.P - 1, primes.Q - 1));

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
