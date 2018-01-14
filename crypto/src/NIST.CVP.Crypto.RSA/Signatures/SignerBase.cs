using System;
using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA2;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public abstract class SignerBase : ISignerBase
    {
        protected int _saltLen;
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

        // For Moq
        public SignerBase()
        {
            _entropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
        }

        public void SetHashFunction(HashFunction hf)
        {
            _hashFunction = hf;
        }

        public void SetSaltLen(int saltLen)
        {
            _saltLen = saltLen;
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

        public SignatureResult SignWithErrors(int nlen, BitString message, IKeyPair key, FailureReasons reason)
        {
            switch (reason)
            {
                case FailureReasons.NONE:
                    return Sign(nlen, message, key);

                case FailureReasons.E:
                    var newKey = new KeyPair(key.PrivKey.P, key.PrivKey.Q, key.PubKey.E + 2);
                    return Sign(nlen, message, key);

                case FailureReasons.MESSAGE:
                    var newMessage = message.ToPositiveBigInteger() + 2;
                    return Sign(nlen, new BitString(newMessage), key);

                case FailureReasons.SIGNATURE:
                    var sigResult = Sign(nlen, message, key);
                    if (sigResult.Success)
                    {
                        sigResult = new SignatureResult(new BitString(sigResult.Signature.ToPositiveBigInteger() + 2));
                    }
                    return sigResult;

                case FailureReasons.IR_MOVED:
                    return MoveIRSign(nlen, message, key);

                case FailureReasons.IR_TRAILER:
                    return ModifyIRTrailerSign(nlen, message, key);

                default:
                    return new SignatureResult("Could not find error type (including NONE)");
            }
        }

        public abstract SignatureResult Sign(int nlen, BitString message, IKeyPair key);
        public abstract VerifyResult Verify(int nlen, BitString signature, IKeyPair key, BitString message);
        public abstract SignatureResult MoveIRSign(int nlen, BitString message, IKeyPair key);
        public abstract SignatureResult ModifyIRTrailerSign(int nlen, BitString message, IKeyPair key);
    }
}
