using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.Signatures.Ansx
{
    public class AnsxPadder : IPaddingScheme
    {
        protected readonly ISha Sha;

        // 4 bits, 0110
        protected readonly BitString Header = new BitString("60", 4);

        // 8 bits, 1100 1100
        protected readonly BitString Tail = new BitString("CC");

        protected readonly BitString A = new BitString("A0", 4);
        protected readonly BitString B = new BitString("B0", 4);

        public AnsxPadder(ISha sha)
        {
            Sha = sha;
        }

        // Override with good or bad padding methods
        public virtual PaddingResult Pad(int nlen, BitString message)
        {
            // 1. Message Hashing
            var hashedMessage = Sha.HashMessage(message).Digest;

            // 2. Hash Encapsulation
            var trailer = GetTrailer();

            // Header is always 4, trailer is always 16
            var paddingLen = nlen - Header.BitLength - Sha.HashFunction.OutputLen - trailer.BitLength;
            var padding = GetPadding(paddingLen);

            var IR = Header.GetDeepCopy();
            IR = BitString.ConcatenateBits(IR, padding);
            IR = BitString.ConcatenateBits(IR, hashedMessage);
            IR = BitString.ConcatenateBits(IR, trailer);

            if (IR.BitLength != nlen)
            {
                return new PaddingResult("Improper length for IR");
            }

            return new PaddingResult(IR);
        }

        public VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey)
        {
            // 1. Signature Opening
            BigInteger irPrime;
            if (embededMessage % 16 == 12)
            {
                irPrime = embededMessage;
            }
            else if ((pubKey.N - embededMessage) % 16 == 12)
            {
                irPrime = pubKey.N - embededMessage;
            }
            else
            {
                return new VerifyResult("Reject signature, failing modulo check");
            }

            if (irPrime < NumberTheory.Pow2(nlen - 2) || irPrime > NumberTheory.Pow2(nlen - 1) - 1)
            {
                return new VerifyResult("irPrime not within required range");
            }

            // 2. Encapsulated Hash Verification
            var bsIrPrime = new BitString(irPrime, nlen);
            if (bsIrPrime.BitLength != nlen)
            {
                return new VerifyResult("Bad bitlength for irPrime");
            }

            if(!bsIrPrime.GetMostSignificantBits(4).Equals(Header))
            {
                return new VerifyResult("Header not found within first 4 bits");
            }

            if (!bsIrPrime.GetLeastSignificantBits(8).Equals(Tail))
            {
                return new VerifyResult("Tail not found within last 8 bits");
            }

            // check nibbles for Bs and A
            var expectedPaddingLen = nlen - Header.BitLength - Sha.HashFunction.OutputLen - GetTrailer().BitLength;
            var expectedPadding = GetPadding(expectedPaddingLen);
            var padding = bsIrPrime.MSBSubstring(4, expectedPaddingLen);

            if (!padding.Equals(expectedPadding))
            {
                return new VerifyResult("Improper padding, must be 'B's followed by 'A'");
            }

            var beginOfHashIndex = expectedPaddingLen + 4;

            // 3. Hash Recovery
            var hashDigest = bsIrPrime.MSBSubstring(beginOfHashIndex, Sha.HashFunction.OutputLen);

            // 4. Message Hashing and Comparison
            var expectedHash = Sha.HashMessage(message).Digest;

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

        public virtual BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return BigInteger.Min(signature, pubKey.N - signature);
        }

        public virtual (PublicKey key, BitString message, int nlen) PrePadCheck(PublicKey key, BitString message, int nlen)
        {
            // TODO maybe use this to check N bitlength on nlen
            return (key, message, nlen);
        }

        protected BitString GetPadding(int len)
        {
            var padding = new BitString(0);

            // Need a bunch of 'b'
            for(var i = 0; i < len - 4; i += 4)
            {
                padding = BitString.ConcatenateBits(padding, B);
            }

            // Just a single 'a'
            padding = BitString.ConcatenateBits(padding, A);

            return padding;
        }

        protected BitString GetTrailer()
        {
            // 1 bit, "0"
            // 3 bits, part number of ISO/IEC 10118 from HashFunction
            // 4 bits, part number of ISO/IEC 10118 from HashFunction
            var trailer = ShaAttributes.HashFunctionToBits(Sha.HashFunction.DigestSize);

            // 8 bits, "CC"
            trailer = BitString.ConcatenateBits(trailer, Tail);
            return trailer;
        }
    }
}
