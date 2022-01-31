using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    public class NativeFastSha2_512_224 : NativeFastSha2LargeBlockBase, ISha
    {
        public HashFunction HashFunction => new HashFunction(ModeValues.SHA2, DigestSizes.d512t224);

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[64];

            Init();
            Update(message.GetPaddedBytes(), message.BitLength);
            Final(digest);

            Array.Resize(ref digest, 28);
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
            Array.Resize(ref digest, 28);
            return new HashResult(new BitString(digest));
        }

        public new void Init()
        {
            H1 = 0x8C3D37C819544DA2;
            H2 = 0x73E1996689DCD4D6;
            H3 = 0x1DFAB7AE32FF9C82;
            H4 = 0x679DD514582F9FCF;
            H5 = 0x0F6D2B697BD44DA8;
            H6 = 0x77E36F7304C48942;
            H7 = 0x3F9D85A86A1D36C8;
            H8 = 0x1112E6AD91D692A1;

            base.Init();
        }

        public void Final(byte[] output, int outputBitLength = 0)
        {
            Finish();

            NativeFastShaUtils.UInt64_To_BE(H1, output, 0);
            NativeFastShaUtils.UInt64_To_BE(H2, output, 8);
            NativeFastShaUtils.UInt64_To_BE(H3, output, 16);
            NativeFastShaUtils.UInt64_To_BE(H4, output, 24);

            // Extra stuff is given, so we clear out the unneeded parts
            Array.Clear(output, 28, output.Length - 28);
        }
    }
}
