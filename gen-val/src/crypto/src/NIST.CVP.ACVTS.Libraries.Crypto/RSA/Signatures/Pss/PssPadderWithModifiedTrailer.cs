using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pss
{
    public class PssPadderWithModifiedTrailer : PssPadder
    {
        public PssPadderWithModifiedTrailer(ISha sha, IMaskFunction mask, IEntropyProvider entropy, int saltLength, int outputLen) : base(sha, mask, entropy, saltLength, outputLen) { }

        public override PaddingResult Pad(int nlen, BitString message)
        {
            var emBits = nlen - 1;
            // OutputLen is meaningful when Sha is an XOF
            var mHash = Sha.HashMessage(message, OutputLen).Digest;
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

            var H = Sha.HashMessage(mPrime, OutputLen).Digest;

            // All bit values
            var PS = BitString.Zeroes(emLen * 8 - SaltLength * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, ZeroOne);
            DB = BitString.ConcatenateBits(DB, salt);

            // All bit values
            var dbMask = Mask.Mask(H, (emLen * 8) - H.BitLength - (1 * 8));
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for (var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, new BitString("4C"));        // ERROR: should be 'BC'

            return new PaddingResult(EM);
        }
    }
}
