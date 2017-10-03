using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.Math;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class ANS_X931_Signer : SignerBase
    {
        // 4 bits, 0110
        private readonly BitString _header = new BitString("60", 4);

        // 8 bits, 1100 1100
        private readonly BitString _tail = new BitString("CC");

        private readonly BitString _a = new BitString("A0", 4);
        private readonly BitString _b = new BitString("B0", 4);

        public ANS_X931_Signer(HashFunction hashFunction) : base(hashFunction) { }

        private BitString GetPadding(int len)
        {
            var padding = new BitString(0);

            // Need a bunch of 'b'
            for(var i = 0; i < len - 4; i += 4)
            {
                padding = BitString.ConcatenateBits(padding, _b);
            }

            // Just a single 'a'
            padding = BitString.ConcatenateBits(padding, _a);

            return padding;
        }

        private BitString GetTrailer()
        {
            // 1 bit, "0"
            // 3 bits, part number of ISO/IEC 10118 from HashFunction
            // 4 bits, part number of ISO/IEC 10118 from HashFunction
            var trailer = SHAEnumHelpers.HashFunctionToBits(_hashFunction);

            // 8 bits, "CC"
            trailer = BitString.ConcatenateBits(trailer, _tail);
            return trailer;
        }

        public override SignatureResult Sign(int nlen, BitString message, KeyPair key)
        {
            // 1. Message Hashing
            var hashedMessage = Hash(message);

            // 2. Hash Encapsulation
            var trailer = GetTrailer();
            // Header is always 4, trailer is always 16
            int paddingLen = nlen - _header.BitLength - SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize) - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            var IR = _header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, trailer);

            if(IR.BitLength != nlen)
            {
                return new SignatureResult("Improper length for IR");
            }

            // 3. Signature Production
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, IR.ToPositiveBigInteger());
            signature =  BigInteger.Min(signature, key.PubKey.N - signature);

            var bsSignature = new BitString(signature, nlen);
            if(bsSignature.BitLength == nlen && bsSignature.GetMostSignificantBits(1) == BitString.Zero())
            {
                return new SignatureResult("Signature bitlength must be nlen - 1");
            }

            // 4. Signature Validation (Optional)
            var verifyResult = Verify(nlen, bsSignature, key, message);
            if(verifyResult.Success)
            {
                return new SignatureResult(bsSignature);
            }
            else
            {
                return new SignatureResult($"Can not verify signature: {verifyResult.ErrorMessage}");
            }
        }

        public override VerifyResult Verify(int nlen, BitString signature, KeyPair key, BitString message)
        {
            // 1. Signature Opening
            var rrPrime = Encrypt(key.PubKey.N, key.PubKey.E, signature.ToPositiveBigInteger());
            BigInteger irPrime;
            BigInteger sig = signature.ToPositiveBigInteger();

            if (rrPrime % 16 == 12)
            {
                irPrime = rrPrime;
            }
            else if((key.PubKey.N - rrPrime) % 16 == 12)
            {
                irPrime = key.PubKey.N - rrPrime;
            }
            else
            {
                return new VerifyResult("Reject signature, failing modulo check");
            }

            if(irPrime < NumberTheory.Pow2(nlen - 2) || irPrime > NumberTheory.Pow2(nlen - 1) - 1)
            {
                return new VerifyResult("irPrime not within required range");
            }

            // 2. Encapsulated Hash Verification
            var bsIRPrime = new BitString(irPrime, nlen);
            if(bsIRPrime.BitLength != nlen)
            {
                return new VerifyResult("Bad bitlength for irPrime");
            }

            if(!bsIRPrime.GetMostSignificantBits(4).Equals(_header))
            {
                return new VerifyResult("Header not found within first 4 bits");
            }

            if(!bsIRPrime.GetLeastSignificantBits(8).Equals(_tail))
            {
                return new VerifyResult("Tail not found within last 8 bits");
            }

            // check nibbles for Bs and A
            var expectedPaddingLen = nlen - _header.BitLength - SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize) - GetTrailer().BitLength;
            var expectedPadding = GetPadding(expectedPaddingLen);
            var padding = bsIRPrime.MSBSubstring(4, expectedPaddingLen);

            if(!padding.Equals(expectedPadding))
            {
                return new VerifyResult("Improper padding, must be 'B's followed by 'A'");
            }

            var beginOfHashIndex = expectedPaddingLen + 4;

            // 3. Hash Recovery
            var hashDigest = bsIRPrime.MSBSubstring(beginOfHashIndex, SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize));

            // 4. Message Hashing and Comparison
            var expectedHash = Hash(message);

            // check trailer for accuracy, including hash function
            var expectedTrailer = GetTrailer();
            var trailer = bsIRPrime.GetLeastSignificantBits(expectedTrailer.BitLength);

            if (!expectedTrailer.Equals(trailer))
            {
                return new VerifyResult("Trailer hash functions do not match, bad signature");
            }

            if(expectedHash.Equals(hashDigest))
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("Hashes do not match, bad signature");
            }
        }

        public override SignatureResult ModifyIRTrailerSign(int nlen, BitString message, KeyPair key)
        {
            _entropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);

            // 1. Message Hashing
            var hashedMessage = Hash(message);

            // 2. Hash Encapsulation
            var trailer = GetTrailer();
            // Header is always 4, trailer is always 16, but this value is 1 byte less than it should be
            int paddingLen = nlen - _header.BitLength - SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize) - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            var IR = _header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, new BitString("66CC"));      // ERROR: Change the trailer to something else

            if (IR.BitLength != nlen)
            {
                return new SignatureResult("Improper length for IR");
            }

            // 3. Signature Production
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, IR.ToPositiveBigInteger());
            signature = BigInteger.Min(signature, key.PubKey.N - signature);

            var bsSignature = new BitString(signature, nlen);
            if (bsSignature.BitLength == nlen && bsSignature.GetMostSignificantBits(1) == BitString.Zero())
            {
                return new SignatureResult("Signature bitlength must be nlen - 1");
            }

            return new SignatureResult(bsSignature);
        }

        public override SignatureResult MoveIRSign(int nlen, BitString message, KeyPair key)
        {
            _entropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);

            // 1. Message Hashing
            var hashedMessage = Hash(message);

            // 2. Hash Encapsulation
            var trailer = GetTrailer();
            // Header is always 4, trailer is always 16, but this value is 1 byte less than it should be, (ERROR is the - 8)
            int paddingLen = nlen - _header.BitLength - SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize) - trailer.BitLength - 8;
            var padding = GetPadding(paddingLen);

            var IR = _header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, _entropy.GetEntropy(8));    // ERROR: Adds back in random bits from the padding that was removed
            IR = BitString.ConcatenateBits(IR, trailer);

            if (IR.BitLength != nlen)
            {
                return new SignatureResult("Improper length for IR");
            }

            // 3. Signature Production
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, IR.ToPositiveBigInteger());
            signature = BigInteger.Min(signature, key.PubKey.N - signature);

            var bsSignature = new BitString(signature, nlen);
            if (bsSignature.BitLength == nlen && bsSignature.GetMostSignificantBits(1) == BitString.Zero())
            {
                return new SignatureResult("Signature bitlength must be nlen - 1");
            }

            return new SignatureResult(bsSignature);
        }
    }
}
