using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA2;
using System.Numerics;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public abstract class SignerBase : ISigner
    {
        protected readonly int _saltLen;
        protected HashFunction _hashFunction;
        private readonly ISHA _sha = new SHA();

        protected IEntropyProvider _entropy;
        protected IEntropyProviderFactory _entropyProviderFactory = new EntropyProviderFactory();

        public SignerBase(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random, int saltLen = 0)
        {
            _hashFunction = hashFunction;
            _entropy = _entropyProviderFactory.GetEntropyProvider(entropyType);
            _saltLen = saltLen;
        }

        public void SetHashFunction(HashFunction hf)
        {
            _hashFunction = hf;
        }

        // Used for salt
        public void AddEntropy(BitString entropy)
        {
            _entropy.AddEntropy(entropy);
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
