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
    public class NativeFastSha1 : NativeFastSha2SmallBlockBase, ISha
    {
        public HashFunction HashFunction { get; } = new HashFunction(ModeValues.SHA1, DigestSizes.d160);

        private uint H1, H2, H3, H4, H5;
        private uint[] X = new uint[80];
        private int xOff;

        private const uint Y1 = 0x5a827999;
        private const uint Y2 = 0x6ed9eba1;
        private const uint Y3 = 0x8f1bbcdc;
        private const uint Y4 = 0xca62c1d6;

        private static uint F(uint u, uint v, uint w) => (u & v) | (~u & w);
        private static uint H(uint u, uint v, uint w) => u ^ v ^ w;
        private static uint G(uint u, uint v, uint w) => (u & v) | (u & w) | (v & w);

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[20];

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
            var digest = new byte[20];
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
            H1 = 0x67452301;
            H2 = 0xefcdab89;
            H3 = 0x98badcfe;
            H4 = 0x10325476;
            H5 = 0xc3d2e1f0;

            xOff = 0;
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
        }

        protected override void ProcessBlock()
        {
            for (int i = 16; i < 80; i++)
            {
                uint t = X[i - 3] ^ X[i - 8] ^ X[i - 14] ^ X[i - 16];
                X[i] = t << 1 | t >> 31;
            }

            uint A = H1;
            uint B = H2;
            uint C = H3;
            uint D = H4;
            uint E = H5;

            //
            // round 1
            //
            int idx = 0;

            for (int j = 0; j < 4; j++)
            {
                E += (A << 5 | (A >> 27)) + F(B, C, D) + X[idx++] + Y1;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + F(A, B, C) + X[idx++] + Y1;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + F(E, A, B) + X[idx++] + Y1;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + F(D, E, A) + X[idx++] + Y1;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + F(C, D, E) + X[idx++] + Y1;
                C = C << 30 | (C >> 2);
            }

            //
            // round 2
            //
            for (int j = 0; j < 4; j++)
            {
                E += (A << 5 | (A >> 27)) + H(B, C, D) + X[idx++] + Y2;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + H(A, B, C) + X[idx++] + Y2;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + H(E, A, B) + X[idx++] + Y2;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + H(D, E, A) + X[idx++] + Y2;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + H(C, D, E) + X[idx++] + Y2;
                C = C << 30 | (C >> 2);
            }

            //
            // round 3
            //
            for (int j = 0; j < 4; j++)
            {
                E += (A << 5 | (A >> 27)) + G(B, C, D) + X[idx++] + Y3;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + G(A, B, C) + X[idx++] + Y3;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + G(E, A, B) + X[idx++] + Y3;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + G(D, E, A) + X[idx++] + Y3;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + G(C, D, E) + X[idx++] + Y3;
                C = C << 30 | (C >> 2);
            }

            //
            // round 4
            //
            for (int j = 0; j < 4; j++)
            {
                E += (A << 5 | (A >> 27)) + H(B, C, D) + X[idx++] + Y4;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + H(A, B, C) + X[idx++] + Y4;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + H(E, A, B) + X[idx++] + Y4;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + H(D, E, A) + X[idx++] + Y4;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + H(C, D, E) + X[idx++] + Y4;
                C = C << 30 | (C >> 2);
            }

            H1 += A;
            H2 += B;
            H3 += C;
            H4 += D;
            H5 += E;

            xOff = 0;
            Array.Clear(X, 0, 16);
        }

        protected override void ProcessLength(long bitLength)
        {
            if (xOff > 14)
            {
                ProcessBlock();
            }

            X[14] = (uint)((ulong)bitLength >> 32);
            X[15] = (uint)((ulong)bitLength);
        }

        protected override void ProcessWord(byte[] input, int inOff)
        {
            X[xOff] = NativeFastShaUtils.BE_To_UInt32(input, inOff);

            if (++xOff == 16)
            {
                ProcessBlock();
            }
        }

        protected override void ProcessWord(uint input)
        {
            X[xOff] = input;

            if (++xOff == 16)
            {
                ProcessBlock();
            }
        }
    }
}
