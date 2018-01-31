using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pss
{
    public class PssPadder : IPaddingScheme
    {
        protected readonly ISha Sha;
        protected readonly IEntropyProvider EntropyProvider;
        protected readonly int SaltLength;

        protected readonly BitString Bc = new BitString("BC");
        protected readonly BitString ZeroOne = new BitString("01");

        public PssPadder(ISha sha, IEntropyProvider entropy, int saltLength)
        {
            Sha = sha;
            EntropyProvider = entropy;
            SaltLength = saltLength;
        }

        public PaddingResult Pad(int nlen, BitString message)
        {
            return EmsaPssEncode(message, nlen - 1);
        }

        public VerifyResult VerifyPadding(int nlen, BitString message, BigInteger embededMessage, PublicKey pubKey)
        {
            return EmsaPssVerify(message, new BitString(embededMessage, nlen), nlen - 1);
        }

        public BigInteger PostSignCheck(BigInteger signature, PublicKey pubKey)
        {
            return signature;
        }

        // B.2.1 Mask Generation Function 1 from RFC-3447
        protected BitString Mgf(BitString seed, int maskLen)
        {
            var T = new BitString(0);
            var iterations = maskLen.CeilingDivide(Sha.HashFunction.OutputLen) - 1;

            for (BigInteger i = 0; i <= iterations; i++)
            {
                var dig = Sha.HashMessage(BitString.ConcatenateBits(seed, new BitString(i, 32))).Digest;
                T = BitString.ConcatenateBits(T, dig);
            }

            return T.MSBSubstring(0, maskLen);
        }

        // 9.1.1 Encoding Operation from RFC-3447
        private PaddingResult EmsaPssEncode(BitString message, int emBits)
        {
            var mHash = Sha.HashMessage(message).Digest;
            var emLen = emBits.CeilingDivide(8);

            // All byte values
            if (emLen < mHash.BitLength / 8 + SaltLength + 2)
            {
                return new PaddingResult("Encoding error");
            }

            var salt = EntropyProvider.GetEntropy(SaltLength * 8);
            var mPrime = BitString.Zeroes(64);
            mPrime = BitString.ConcatenateBits(mPrime, mHash);
            mPrime = BitString.ConcatenateBits(mPrime, salt);

            var H = Sha.HashMessage(mPrime).Digest;

            // All bit values
            var PS = BitString.Zeroes(emLen * 8 - SaltLength * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, ZeroOne);
            DB = BitString.ConcatenateBits(DB, salt);

            // All bit values
            var dbMask = Mgf(H, emLen * 8 - H.BitLength - 1 * 8);
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for (var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, Bc);

            return new PaddingResult(EM);
        }

        private VerifyResult EmsaPssVerify(BitString M, BitString EM, int emBits)
        {
            var mHash = Sha.HashMessage(M).Digest;
            if (EM.BitLength < mHash.BitLength / 8 + SaltLength + 2)
            {
                return new VerifyResult("RSA PSS Verify: inconsistent result");
            }

            if (!EM.Substring(0, 8).Equals(Bc))
            {
                return new VerifyResult("RSA PSS Verify: 'BC' hex not found");
            }

            // Bit values 
            var maskedDB = EM.MSBSubstring(0, EM.BitLength - mHash.BitLength - 8);
            var H = EM.MSBSubstring(maskedDB.BitLength, mHash.BitLength);

            if (!maskedDB.MSBSubstring(0, EM.BitLength - emBits).Equals(BitString.Zeroes(EM.BitLength - emBits)))
            {
                return new VerifyResult("RSA PSS Verify: Leading 0s not found in maskedDB");
            }

            var dbMask = Mgf(H, EM.BitLength - mHash.BitLength - 8);
            var DB = BitString.XOR(maskedDB, dbMask);

            for (var i = 0; i < EM.BitLength - emBits; i++)
            {
                DB.Set(DB.BitLength - i - 1, false);
            }

            var leftmostOctets = EM.BitLength - mHash.BitLength - SaltLength * 8 - 16;

            if(leftmostOctets != 0)
            {
                if (!DB.MSBSubstring(0, leftmostOctets).Equals(BitString.Zeroes(leftmostOctets)))
                {
                    return new VerifyResult("RSA PSS Verify: DB incorrect, improper number of leading 0s");
                }
            }

            if (!DB.MSBSubstring(leftmostOctets, 8).Equals(ZeroOne))
            {
                return new VerifyResult("RSA PSS Verify: DB incorrect, '01' byte not found");
            }

            var salt = DB.Substring(0, SaltLength * 8);
            var MPrime = BitString.Zeroes(64);
            MPrime = BitString.ConcatenateBits(MPrime, mHash);
            MPrime = BitString.ConcatenateBits(MPrime, salt);

            var HPrime = Sha.HashMessage(MPrime).Digest;

            if (HPrime.Equals(H))
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("RSA PSS Verify: Hashes do not match");
            }
        }

        public (PublicKey key, BitString message, int nlen) PrePadCheck(PublicKey key, BitString message, int nlen)
        {
            return (key, message, nlen);
        }
    }
}
