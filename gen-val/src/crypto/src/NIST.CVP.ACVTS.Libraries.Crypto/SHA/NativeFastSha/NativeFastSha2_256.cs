using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    /// <summary>
    /// Heavily borrowed from BouncyCastle via https://github.com/bcgit/bc-csharp 
    /// </summary>
    public class NativeFastSha2_256 : NativeFastSha2SmallBlockBase, ISha
    {
        public HashFunction HashFunction { get; } = new HashFunction(ModeValues.SHA2, DigestSizes.d256);

        private uint H1, H2, H3, H4, H5, H6, H7, H8;
        private uint[] X = new uint[64];
        private int xOffset;

        private static readonly uint[] K = {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
            0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
            0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
            0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
            0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
            0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
            0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
            0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
            0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        private static uint Sum1Ch(uint x, uint y, uint z) => (((x >> 6) | (x << 26)) ^ ((x >> 11) | (x << 21)) ^ ((x >> 25) | (x << 7))) + (z ^ (x & (y ^ z)));
        private static uint Sum0Maj(uint x, uint y, uint z) => (((x >> 2) | (x << 30)) ^ ((x >> 13) | (x << 19)) ^ ((x >> 22) | (x << 10))) + ((x & y) | (z & (x ^ y)));
        private static uint Theta0(uint x) => ((x >> 7) | (x << 25)) ^ ((x >> 18) | (x << 14)) ^ (x >> 3);
        private static uint Theta1(uint x) => ((x >> 17) | (x << 15)) ^ ((x >> 19) | (x << 13)) ^ (x >> 10);

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[32];

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
            var digest = new byte[32];
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
            H1 = 0x6a09e667;
            H2 = 0xbb67ae85;
            H3 = 0x3c6ef372;
            H4 = 0xa54ff53a;
            H5 = 0x510e527f;
            H6 = 0x9b05688c;
            H7 = 0x1f83d9ab;
            H8 = 0x5be0cd19;

            xOffset = 0;
            Array.Clear(X, 0, X.Length);
            base.Init();
        }

        public void Final(byte[] output, int outputBitLength = 0)
        {
            Finish();

            NativeFastShaUtils.UInt32_To_BE(H1, output, 0);
            NativeFastShaUtils.UInt32_To_BE(H2, output, 4);
            NativeFastShaUtils.UInt32_To_BE(H3, output, 8);
            NativeFastShaUtils.UInt32_To_BE(H4, output, 12);
            NativeFastShaUtils.UInt32_To_BE(H5, output, 16);
            NativeFastShaUtils.UInt32_To_BE(H6, output, 20);
            NativeFastShaUtils.UInt32_To_BE(H7, output, 24);
            NativeFastShaUtils.UInt32_To_BE(H8, output, 28);
        }

        protected override void ProcessBlock()
        {
            for (int ti = 16; ti <= 63; ti++)
            {
                X[ti] = Theta1(X[ti - 2]) + X[ti - 7] + Theta0(X[ti - 15]) + X[ti - 16];
            }

            uint a = H1;
            uint b = H2;
            uint c = H3;
            uint d = H4;
            uint e = H5;
            uint f = H6;
            uint g = H7;
            uint h = H8;

            int t = 0;
            for (int i = 0; i < 8; ++i)
            {
                // t = 8 * i
                h += Sum1Ch(e, f, g) + K[t] + X[t];
                d += h;
                h += Sum0Maj(a, b, c);
                ++t;

                // t = 8 * i + 1
                g += Sum1Ch(d, e, f) + K[t] + X[t];
                c += g;
                g += Sum0Maj(h, a, b);
                ++t;

                // t = 8 * i + 2
                f += Sum1Ch(c, d, e) + K[t] + X[t];
                b += f;
                f += Sum0Maj(g, h, a);
                ++t;

                // t = 8 * i + 3
                e += Sum1Ch(b, c, d) + K[t] + X[t];
                a += e;
                e += Sum0Maj(f, g, h);
                ++t;

                // t = 8 * i + 4
                d += Sum1Ch(a, b, c) + K[t] + X[t];
                h += d;
                d += Sum0Maj(e, f, g);
                ++t;

                // t = 8 * i + 5
                c += Sum1Ch(h, a, b) + K[t] + X[t];
                g += c;
                c += Sum0Maj(d, e, f);
                ++t;

                // t = 8 * i + 6
                b += Sum1Ch(g, h, a) + K[t] + X[t];
                f += b;
                b += Sum0Maj(c, d, e);
                ++t;

                // t = 8 * i + 7
                a += Sum1Ch(f, g, h) + K[t] + X[t];
                e += a;
                a += Sum0Maj(b, c, d);
                ++t;
            }

            H1 += a;
            H2 += b;
            H3 += c;
            H4 += d;
            H5 += e;
            H6 += f;
            H7 += g;
            H8 += h;

            xOffset = 0;
            Array.Clear(X, 0, 16);
        }

        protected override void ProcessLength(long bitLength)
        {
            if (xOffset > 14)
            {
                ProcessBlock();
            }

            X[14] = (uint)((ulong)bitLength >> 32);
            X[15] = (uint)((ulong)bitLength);
        }

        protected override void ProcessWord(byte[] input, int inOff)
        {
            X[xOffset] = NativeFastShaUtils.BE_To_UInt32(input, inOff);

            if (++xOffset == 16)
            {
                ProcessBlock();
            }
        }

        protected override void ProcessWord(uint input)
        {
            X[xOffset] = input;

            if (++xOffset == 16)
            {
                ProcessBlock();
            }
        }
    }
}
