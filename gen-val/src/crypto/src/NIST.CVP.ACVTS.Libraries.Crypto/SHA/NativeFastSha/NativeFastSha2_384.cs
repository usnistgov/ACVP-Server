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
    public class NativeFastSha2_384 : NativeFastSha2LargeBlockBase, ISha
    {
        public HashFunction HashFunction => new HashFunction(ModeValues.SHA2, DigestSizes.d384);

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[48];

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
            var digest = new byte[48];
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
            H1 = 0xcbbb9d5dc1059ed8;
            H2 = 0x629a292a367cd507;
            H3 = 0x9159015a3070dd17;
            H4 = 0x152fecd8f70e5939;
            H5 = 0x67332667ffc00b31;
            H6 = 0x8eb44a8768581511;
            H7 = 0xdb0c2e0d64f98fa7;
            H8 = 0x47b5481dbefa4fa4;

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
        }
    }
}
