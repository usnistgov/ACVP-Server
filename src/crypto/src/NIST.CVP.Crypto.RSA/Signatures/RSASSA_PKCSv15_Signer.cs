using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class RSASSA_PKCSv15_Signer : SignerBase
    {
        private readonly BitString _sha1AlgId = new BitString("30 21 30 09 06 05 2b 0e 03 02 1a 05 00 04 14");
        private readonly BitString _sha224AlgId = new BitString("30 2d 30 0d 06 09 60 86 48 01 65 03 04 02 04 05 00 04 1c");
        private readonly BitString _sha256AlgId = new BitString("30 31 30 0d 06 09 60 86 48 01 65 03 04 02 01 05 00 04 20");
        private readonly BitString _sha384AlgId = new BitString("30 41 30 0d 06 09 60 86 48 01 65 03 04 02 02 05 00 04 30");
        private readonly BitString _sha512AlgId = new BitString("30 51 30 0d 06 09 60 86 48 01 65 03 04 02 03 05 00 04 40");
        private readonly BitString _sha512224AlgId = new BitString("30 2d 30 0d 06 09 60 86 48 01 65 03 04 02 05 05 00 04 1c");
        private readonly BitString _sha512256AlgId = new BitString("30 31 30 0d 06 09 60 86 48 01 65 03 04 02 06 05 00 04 20");

        public RSASSA_PKCSv15_Signer(HashFunction hashFunction) : base(hashFunction) { }

        private BitString GetHashAlgId()
        {
            switch (_hashFunction.DigestSize)
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

        private BitString EMSA_PKCS_Encoding(BitString message, int nlen)
        {
            var H = Hash(message);
            var T = BitString.ConcatenateBits(GetHashAlgId(), H);

            var psLen = nlen - (GetHashAlgId().BitLength + SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize)) - 24;
            var PS = BitString.Ones(psLen);

            var EM = new BitString("00");
            EM = BitString.ConcatenateBits(EM, new BitString("01"));
            EM = BitString.ConcatenateBits(EM, PS);
            EM = BitString.ConcatenateBits(EM, new BitString("00"));
            EM = BitString.ConcatenateBits(EM, T);

            return EM;
        }

        public override SignatureResult Sign(int nlen, BitString message, IKeyPair key)
        {
            // 1. Apply EMSA-PKCS1-v1.5 Encoding
            var EM = EMSA_PKCS_Encoding(message, nlen);
            if (message.BitLength < GetHashAlgId().BitLength + 11 * 8)
            {
                return new SignatureResult("Message length too short");
            }

            // 2. RSA Signature
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());

            // 3. Output Signature
            return new SignatureResult(new BitString(signature, nlen));
        }

        public override VerifyResult Verify(int nlen, BitString signature, IKeyPair key, BitString message)
        {
            // 1. Length Checking
            if(signature.BitLength != nlen)
            {
                return new VerifyResult("Signature is not nlen in length");
            }

            if (key.PubKey.E.IsEven)
            {
                return new VerifyResult("E must be odd");
            }

            // 2. RSA Verification
            var EM = Encrypt(key.PubKey.N, key.PubKey.E, signature.ToPositiveBigInteger());

            // 3. Encoded Message Check
            var EMPrime = EMSA_PKCS_Encoding(message, nlen).ToPositiveBigInteger();

            // Check BigIntegers instead of BitStrings because EM comes out with slightly less bits (0 bits from the front)
            if (EM == EMPrime)
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("Encoded messages do not match");
            }
        }

        public override SignatureResult ModifyIRTrailerSign(int nlen, BitString message, IKeyPair key)
        {
            // 1. Encode Message
            var H = Hash(message);
            var T = BitString.ConcatenateBits(GetHashAlgId(), H);

            var psLen = nlen - (GetHashAlgId().BitLength + SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize)) - 24;
            var PS = BitString.Ones(psLen);

            var EM = new BitString("00");
            EM = BitString.ConcatenateBits(EM, new BitString("01"));
            EM = BitString.ConcatenateBits(EM, PS);
            EM = BitString.ConcatenateBits(EM, new BitString("44"));        // ERROR: Should be 00
            EM = BitString.ConcatenateBits(EM, T);

            if (message.BitLength < GetHashAlgId().BitLength + 11 * 8)
            {
                return new SignatureResult("Message length too short");
            }

            // 2. RSA Signature
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());

            // 3. Output Signature
            return new SignatureResult(new BitString(signature));
        }

        public override SignatureResult MoveIRSign(int nlen, BitString message, IKeyPair key)
        {
            var _rand = new Random800_90();

            // 1. Encode Message
            var H = Hash(message);
            var T = BitString.ConcatenateBits(GetHashAlgId(), H);

            var psLen = nlen - (GetHashAlgId().BitLength + SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize)) - 24;
            var PS = BitString.Ones(psLen);

            var EM = new BitString("00");
            EM = BitString.ConcatenateBits(EM, new BitString("01"));
            EM = BitString.ConcatenateBits(EM, PS);
            EM = BitString.ConcatenateBits(EM, BitString.Ones(_rand.GetRandomInt(1, PS.BitLength)));    // ERROR: Add random amount of 1s to PS
            EM = BitString.ConcatenateBits(EM, new BitString("00"));
            EM = BitString.ConcatenateBits(EM, T);

            if (message.BitLength < GetHashAlgId().BitLength + 11 * 8)
            {
                return new SignatureResult("Message length too short");
            }

            // 2. RSA Signature
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());

            // 3. Output Signature
            return new SignatureResult(new BitString(signature, nlen));
        }
    }
}
