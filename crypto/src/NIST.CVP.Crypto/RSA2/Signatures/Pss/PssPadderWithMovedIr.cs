using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.Signatures.Pss
{
    public class PssPadderWithMovedIr : PssPadder
    {
        public PssPadderWithMovedIr(ISha sha, IEntropyProvider entropy, int saltLength) : base(sha, entropy, saltLength) { }

        public override PaddingResult Pad(int nlen, BitString message)
        {
            var emBits = nlen - 1;
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

            // ERROR: split maskedDB into two chunks and insert hashed message in the middle
            var firstChunkMask = maskedDB.GetMostSignificantBits(maskedDB.BitLength - 8);
            var secondChunkMask = maskedDB.GetLeastSignificantBits(8);

            var EM = firstChunkMask.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, secondChunkMask);
            EM = BitString.ConcatenateBits(EM, Bc);

            return new PaddingResult(EM);
        }
    }
}
