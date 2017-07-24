using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA2;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public abstract class SignerBase : ISigner
    {
        protected readonly int _saltLen;
        protected readonly HashFunction _hashFunction;
        private readonly ISHA _sha = new SHA();
        protected readonly IRandom800_90 _rand = new Random800_90();

        public SignerBase(HashFunction hashFunction)
        {
            _hashFunction = hashFunction;
        }

        public SignerBase(HashFunction hashFunction, int saltLen)
        {
            _hashFunction = hashFunction;
            _saltLen = saltLen;
        }
            
        protected BitString Hash(BitString message)
        {
            var hashResult = _sha.HashMessage(_hashFunction, message);
            if (!hashResult.Success)
            {
                throw new Exception("Bad hash in RSA Signature");
            }
            return hashResult.Digest;
        }

        protected BigInteger Decrypt(BigInteger n, BigInteger d, BigInteger ciphertext)
        {
            return BigInteger.ModPow(ciphertext, d, n);
        }

        protected BigInteger Encrypt(BigInteger n, BigInteger e, BigInteger plaintext)
        {
            return BigInteger.ModPow(plaintext, e, n);
        }

        public abstract SignatureResult Sign(int nlen, BitString message, KeyPair key);
        public abstract VerifyResult Verify(int nlen, BitString signature, KeyPair key, BitString message);
    }
}
