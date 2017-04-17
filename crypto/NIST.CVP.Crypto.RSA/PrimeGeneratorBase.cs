using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    public abstract class PrimeGeneratorBase
    {
        private ISHA _hash;
        private HashFunction _hashFunction;
        private BigInteger _x1;

        public PrimeGeneratorBase()
        {
            _hash = new SHA(new SHAFactory());
            _hashFunction = new HashFunction
            {
                DigestSize = DigestSizes.d224,
                Mode = ModeValues.SHA2
            };
        }

        protected abstract PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BigInteger seed);

        protected HashResult Hash(BitString message, DigestSizes digestSize)
        {
            _hashFunction.DigestSize = digestSize;
            return _hash.HashMessage(_hashFunction, message);
        }
    }
}
