using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pkcs
{
    public class PkcsPadder : IPaddingScheme
    {
        protected readonly ISha Sha;

        private readonly BitString _sha1AlgId = new BitString("30 21 30 09 06 05 2b 0e 03 02 1a 05 00 04 14");
        private readonly BitString _sha224AlgId = new BitString("30 2d 30 0d 06 09 60 86 48 01 65 03 04 02 04 05 00 04 1c");
        private readonly BitString _sha256AlgId = new BitString("30 31 30 0d 06 09 60 86 48 01 65 03 04 02 01 05 00 04 20");
        private readonly BitString _sha384AlgId = new BitString("30 41 30 0d 06 09 60 86 48 01 65 03 04 02 02 05 00 04 30");
        private readonly BitString _sha512AlgId = new BitString("30 51 30 0d 06 09 60 86 48 01 65 03 04 02 03 05 00 04 40");
        private readonly BitString _sha512224AlgId = new BitString("30 2d 30 0d 06 09 60 86 48 01 65 03 04 02 05 05 00 04 1c");
        private readonly BitString _sha512256AlgId = new BitString("30 31 30 0d 06 09 60 86 48 01 65 03 04 02 06 05 00 04 20");

        protected BitString GetHashAlgId()
        {
            switch (Sha.HashFunction.DigestSize)
            {
                case DigestSizes.d160:
                    return _sha1AlgId;
                case DigestSizes.d224:
                    return _sha224AlgId;
                case DigestSizes.d256:
                    return _sha256AlgId;
                case DigestSizes.d384:
                    return _sha384AlgId;
                case DigestSizes.d512:
                    return _sha512AlgId;
                case DigestSizes.d512t224:
                    return _sha512224AlgId;
                case DigestSizes.d512t256:
                    return _sha512256AlgId;
                default:
                    throw new Exception("Bad digest size");
            }
        }

        public PkcsPadder(ISha sha)
        {
            Sha = sha;
        }

        private BitString EmsaPkcsEncoding(BitString message, int nlen)
        {
            var H = Sha.HashMessage(message).Digest;
            var T = BitString.ConcatenateBits(GetHashAlgId(), H);

            var psLen = nlen - (GetHashAlgId().BitLength + Sha.HashFunction.OutputLen) - 24;
            var PS = BitString.Ones(psLen);

            var EM = new BitString("00");
            EM = BitString.ConcatenateBits(EM, new BitString("01"));
            EM = BitString.ConcatenateBits(EM, PS);
            EM = BitString.ConcatenateBits(EM, new BitString("00"));
            EM = BitString.ConcatenateBits(EM, T);

            return EM;
        }

        public virtual PaddingResult Pad(int nlen, BitString message)
        {
            if (message.BitLength < GetHashAlgId().BitLength + 11 * 8)
            {
                return new PaddingResult("Message length too short");
            }

            var embeddedMessage = EmsaPkcsEncoding(message, nlen);
            return new PaddingResult(embeddedMessage);
        }

        public virtual BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return signature;
        }

        public VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey)
        {
            // 3. Encoded Message Check
            var EMPrime = EmsaPkcsEncoding(message, nlen).ToPositiveBigInteger();

            // Check BigIntegers instead of BitStrings because EM comes out with slightly less bits (0 bits from the front)
            if (embededMessage == EMPrime)
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("Encoded messages do not match");
            }
        }

        public virtual (KeyPair key, BitString message, int nlen) PrePadCheck(KeyPair key, BitString message, int nlen)
        {
            return (key, message, nlen);
        }
    }
}
