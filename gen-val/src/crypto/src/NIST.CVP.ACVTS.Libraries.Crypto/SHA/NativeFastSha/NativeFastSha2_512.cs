using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    /// <summary>
    /// Heavily borrowed from https://github.com/bcgit/bc-csharp
    /// </summary>
    public class NativeFastSha2_512 : NativeFastSha2LargeBlockBase, ISha
    {
        public HashFunction HashFunction => new HashFunction(ModeValues.SHA2, DigestSizes.d512);

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[64];

            Init();
            Update(message.GetPaddedBytes(), message.BitLength);
            Final(digest);

            return new HashResult(new BitString(digest));
        }

        public HashResult HashNumber(BigInteger number)
        {
            var bs = new BitString(number);

            // Pad the BitString to be a multiple of 32 bits
            // Likely a relic of old MultiPrecision libraries
            // Spec says to just hash the integer which is normally 4 bytes but 
            //      with larger integer values, libraries keep them at multiples
            //      of 4 bytes, so we have to make sure our structure is a multiple
            //      of 4 bytes as well.
            if (bs.BitLength % 32 != 0)
            {
                bs = BitString.ConcatenateBits(BitString.Zeroes(32 - bs.BitLength % 32), bs);
            }

            return HashMessage(bs);
        }

        public HashResult HashLargeMessage(LargeBitString message)
        {
            var digest = new byte[64];

            var iterations = message.FullLength / message.ContentLength;
            var paddedBytes = message.Content.GetPaddedBytes();

            Init();

            for (var i = 0; i < iterations; i++)
            {
                Update(paddedBytes, message.Content.BitLength);
            }

            Final(digest);

            return new HashResult(new BitString(digest));
        }

        public new void Init()
        {
            H1 = 0x6a09e667f3bcc908;
            H2 = 0xbb67ae8584caa73b;
            H3 = 0x3c6ef372fe94f82b;
            H4 = 0xa54ff53a5f1d36f1;
            H5 = 0x510e527fade682d1;
            H6 = 0x9b05688c2b3e6c1f;
            H7 = 0x1f83d9abfb41bd6b;
            H8 = 0x5be0cd19137e2179;

            base.Init();
        }

        public void Final(byte[] output, int outputBitLength = 0)
        {
            Finish();

            NativeFastShaUtils.UInt64_To_BE(H1, output, 0);
            NativeFastShaUtils.UInt64_To_BE(H2, output, 8);
            NativeFastShaUtils.UInt64_To_BE(H3, output, 16);
            NativeFastShaUtils.UInt64_To_BE(H4, output, 24);
            NativeFastShaUtils.UInt64_To_BE(H5, output, 32);
            NativeFastShaUtils.UInt64_To_BE(H6, output, 40);
            NativeFastShaUtils.UInt64_To_BE(H7, output, 48);
            NativeFastShaUtils.UInt64_To_BE(H8, output, 56);
        }
    }
}
