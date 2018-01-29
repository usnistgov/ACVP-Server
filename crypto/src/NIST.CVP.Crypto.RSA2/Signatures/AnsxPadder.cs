using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class AnsxPadder : IPaddingScheme
    {
        private readonly ISha _sha;

        // 4 bits, 0110
        private readonly BitString _header = new BitString("60", 4);

        // 8 bits, 1100 1100
        private readonly BitString _tail = new BitString("CC");

        private readonly BitString _a = new BitString("A0", 4);
        private readonly BitString _b = new BitString("B0", 4);

        public AnsxPadder(ISha sha)
        {
            _sha = sha;
        }

        public PaddingResult Pad(int nlen, BitString message)
        {
            // 1. Message Hashing
            var hashedMessage = _sha.HashMessage(message).Digest;

            // 2. Hash Encapsulation
            var trailer = GetTrailer();
            // Header is always 4, trailer is always 16
            int paddingLen = nlen - _header.BitLength - _sha.HashFunction.OutputLen - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            var IR = _header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, trailer);

            if(IR.BitLength != nlen)
            {
                return new PaddingResult("Improper length for IR");
            }

            return new PaddingResult(IR);
        }

        public BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return BigInteger.Min(signature, pubKey.N - signature);
        }

        public VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey)
        {
            // 1. Signature Opening
            BigInteger irPrime;
            if (embededMessage % 16 == 12)
            {
                irPrime = embededMessage;
            }
            else if((pubKey.N - embededMessage) % 16 == 12)
            {
                irPrime = pubKey.N - embededMessage;
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
            var bsIrPrime = new BitString(irPrime, nlen);
            if(bsIrPrime.BitLength != nlen)
            {
                return new VerifyResult("Bad bitlength for irPrime");
            }

            if(!bsIrPrime.GetMostSignificantBits(4).Equals(_header))
            {
                return new VerifyResult("Header not found within first 4 bits");
            }

            if(!bsIrPrime.GetLeastSignificantBits(8).Equals(_tail))
            {
                return new VerifyResult("Tail not found within last 8 bits");
            }

            // check nibbles for Bs and A
            var expectedPaddingLen = nlen - _header.BitLength - _sha.HashFunction.OutputLen - GetTrailer().BitLength;
            var expectedPadding = GetPadding(expectedPaddingLen);
            var padding = bsIrPrime.MSBSubstring(4, expectedPaddingLen);

            if(!padding.Equals(expectedPadding))
            {
                return new VerifyResult("Improper padding, must be 'B's followed by 'A'");
            }

            var beginOfHashIndex = expectedPaddingLen + 4;

            // 3. Hash Recovery
            var hashDigest = bsIrPrime.MSBSubstring(beginOfHashIndex, _sha.HashFunction.OutputLen);

            // 4. Message Hashing and Comparison
            var expectedHash = _sha.HashMessage(message).Digest;

            // check trailer for accuracy, including hash function
            var expectedTrailer = GetTrailer();
            var trailer = bsIrPrime.GetLeastSignificantBits(expectedTrailer.BitLength);

            if (!expectedTrailer.Equals(trailer))
            {
                return new VerifyResult("Trailer hash functions do not match, bad signature");
            }

            if (expectedHash.Equals(hashDigest))
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("Hashes do not match, bad signature");
            }
        }

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
            var trailer = ShaAttributes.HashFunctionToBits(_sha.HashFunction.DigestSize);

            // 8 bits, "CC"
            trailer = BitString.ConcatenateBits(trailer, _tail);
            return trailer;
        }
    }
}
