using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System;
using System.Linq;
using System.Numerics;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Ffx
{
    public class Ff1BlockCipher : FfxBlockCipherBase
    {
        public Ff1BlockCipher(IBlockCipherEngine engine, IModeBlockCipherFactory factory, IAesFfInternals ffInternals)
            : base(engine, factory, ffInternals) { }

        protected override int NumberOfRounds => 10;

        protected override BitString Encrypt(IFfxModeBlockCipherParameters param)
        {
            var mode = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);

            var X = NumeralString.ToNumeralString(param.Payload);
            var n = X.Numbers.Length;
            var t = param.Iv.BitLength / BitsInByte;

            // 1. Let u = Floor(n/2); v = n – u.
            var u = n / 2;
            var v = n - u;

            // 2. Let A = X[1..u]; B = X[u + 1..n].
            var A = new NumeralString(X.Numbers.Take(u).ToArray());
            var B = new NumeralString(X.Numbers.Skip(u).Take(v).ToArray());

            // 3. Let b = Ceiling(Ceiling(v×LOG(radix))/8).
            var b = (int)System.Math.Ceiling(System.Math.Ceiling(v * System.Math.Log(param.Radix, 2)) / 8);

            // 4. Let d = 4*Ceiling(b/4) + 4.
            var d = 4 * b.CeilingDivide(4) + 4;

            // the number times to iterate through the concatenation of cipher iterator blocks
            var jCount = d.CeilingDivide(16);
            
            // 5. Let P = [1] 1 || [2] 1 || [1] 1 || [radix] 3 || [10] 1 || [u mod 256] 1 || [n] 4 || [t] 4.
            var pBytes = new byte[_engine.BlockSizeBytes];
            pBytes[0] = 0x01;
            pBytes[1] = 0x02;
            pBytes[2] = 0x01;
            // 3 bytes for radix, but radix is capped at 16 bits (2^16), so add a 0 byte
            pBytes[3] = 0x00;
            Array.Copy(BitString.To32BitString(param.Radix).GetLeastSignificantBits(16).ToBytes(), 0, pBytes, 4, 2);
            pBytes[6] = 0x0a;
            pBytes[7] = (byte)(u % 256);
            Array.Copy(BitString.To32BitString(n).ToBytes(), 0, pBytes, 8, 4);
            Array.Copy(BitString.To32BitString(t).ToBytes(), 0, pBytes, 12, 4);
            var P = new BitString(pBytes);

            // 6. For i from 0 to 9:
            for (var i = 0; i < NumberOfRounds; i++)
            {
                // i. Let Q = T || [0] (−t−b−1) mod 16 || [i] 1 || [NUMradix(B)]b.
                var numB = new BitString(_ffInternals.Num(param.Radix, B));
                if (numB.BitLength < b * BitsInByte)
                {
                    var bitsToPrepend = new BitString(b * BitsInByte - numB.BitLength);
                    numB = bitsToPrepend.ConcatenateBits(numB);
                }

                var Q = new BitString(0)
                    .ConcatenateBits(param.Iv)
                    .ConcatenateBits(new BitString((-t - b - 1).PosMod(16) * BitsInByte))
                    .ConcatenateBits(((byte)i).ToBitString())
                    .ConcatenateBits(numB.GetMostSignificantBits(b * BitsInByte));

                //     ii. Let R = PRF(P || Q).
                var R = _ffInternals.Prf(P.ConcatenateBits(Q), param.Key);

                //     iii. Let S be the first d bytes of the following string of Ceiling(d/16) blocks:
                //         R || CIPHK(R Å [1]16) || CIPHK (R Å [2]16) … CIPHK(R Å [éd/16ù–1]).
                var S = R.GetDeepCopy();
                for (var j = 1; j < jCount; j++)
                {
                    var pad = new BitString(new byte[15])
                        .ConcatenateBits(((byte)j).ToBitString());
                    S = S.ConcatenateBits(
                        mode.ProcessPayload(
                            new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                param.Key,
                                R.XOR(pad))).Result
                    );
                }
                S = S.GetMostSignificantBits(d * BitsInByte);
                
                //     iv. Let y = NUM(S).
                var y = _ffInternals.Num(S);

                //     v. If i is even, let m = u; else, let m = v.
                var m = i % 2 == 0 ? u : v;

                //     vi. Let c = (NUMradix (A)+y) mod radix m .
                var c = (_ffInternals.Num(param.Radix, A) + y).PosMod(BigInteger.Pow(param.Radix, m));

                //     vii. Let C = STR m radix (c).
                var C = _ffInternals.Str(param.Radix, m, c);

                //     viii. Let A = B.
                A = B;
                //     ix. Let B = C.
                B = C;
            }

            // 7. Return A || B.
            return NumeralString.ToBitString(A)
                .ConcatenateBits(NumeralString.ToBitString(B));
        }

        protected override BitString Decrypt(IFfxModeBlockCipherParameters param)
        {
            var mode = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);

            var X = NumeralString.ToNumeralString(param.Payload);
            var n = X.Numbers.Length;
            var t = param.Iv.BitLength / BitsInByte;

            // 1. Let u = Floor(n/2); v = n – u.
            var u = n / 2;
            var v = n - u;

            // 2. Let A = X[1..u]; B = X[u+1..n].
            var A = new NumeralString(X.Numbers.Take(u).ToArray());
            var B = new NumeralString(X.Numbers.Skip(u).Take(v).ToArray());

            // 3. Let b = Ceiling(Ceiling(v×LOG(radix))/8).
            var b = (int)System.Math.Ceiling(System.Math.Ceiling(v * System.Math.Log(param.Radix, 2)) / 8);

            // 4. Let d = 4 Ceiling(b/4)+4
            var d = (4 * b.CeilingDivide(4) + 4);

            // the number times to iterate through the concatenation of cipher iterator blocks
            var jCount = d.CeilingDivide(16);
            
            // 5. Let P = [1] 1 || [2] 1 || [1] 1 || [radix] 3 || [10] 1 ||[u mod 256] 1 || [n] 4 || [t] 4 .
            var pBytes = new byte[_engine.BlockSizeBytes];
            pBytes[0] = 0x01;
            pBytes[1] = 0x02;
            pBytes[2] = 0x01;
            // 3 bytes for radix, but radix is capped at 16 bits (2^16), so add a 0 byte
            pBytes[3] = 0x00;
            Array.Copy(BitString.To32BitString(param.Radix).GetLeastSignificantBits(16).ToBytes(), 0, pBytes, 4, 2);
            pBytes[6] = 0x0a;
            pBytes[7] = (byte)(u % 256);
            Array.Copy(BitString.To32BitString(n).ToBytes(), 0, pBytes, 8, 4);
            Array.Copy(BitString.To32BitString(t).ToBytes(), 0, pBytes, 12, 4);
            var P = new BitString(pBytes);
            
            // 6. For i from 9 to 0:
            for (var i = NumberOfRounds - 1; i >= 0; i--)
            {
                // This step differs from encrypt step, uses A rather than B
                var numA = new BitString(_ffInternals.Num(param.Radix, A));
                if (numA.BitLength < b * BitsInByte)
                {
                    var bitsToPrepend = new BitString(b * BitsInByte - numA.BitLength);
                    numA = bitsToPrepend.ConcatenateBits(numA);
                }

                // 	i. Let Q = T || [0](−t−b−1) mod 16 || [i] 1 || [NUMradix (A)]b.
                var Q = new BitString(0)
                    .ConcatenateBits(param.Iv)
                    .ConcatenateBits(new BitString((-t - b - 1).PosMod(16) * BitsInByte))
                    .ConcatenateBits(((byte)i).ToBitString())
                    .ConcatenateBits(numA.GetMostSignificantBits(b * BitsInByte));

                //     ii. Let R = PRF(P || Q).
                var R = _ffInternals.Prf(P.ConcatenateBits(Q), param.Key);

                //     iii. Let S be the first d bytes of the following string of Ceiling(d/16) blocks:
                //         R || CIPHK(R Å [1]16) || CIPHK (R Å [2]16) … CIPHK(R Å [éd/16ù–1]).
                var S = R.GetDeepCopy();
                for (var j = 1; j < jCount; j++)
                {
                    var pad = new BitString(new byte[15])
                        .ConcatenateBits(((byte)j).ToBitString());
                    S = S.ConcatenateBits(
                        mode.ProcessPayload(
                            new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                param.Key,
                                R.XOR(pad))).Result
                    );
                }

                S = S.GetMostSignificantBits(d * BitsInByte);

                //     iv. Let y = NUM(S).
                var y = _ffInternals.Num(S);

                //     v. If i is even, let m = u; else, let m = v.
                var m = i % 2 == 0 ? u : v;

                //     vi. Let c = (NUMradix (B)-y) mod radix m .
                // This step differs from Encrypt, using B instead of A, also uses modular subtraction rather than addition
                var c = (_ffInternals.Num(param.Radix, B) - y).PosMod(BigInteger.Pow(param.Radix, m));

                //     vii. Let C = STR m radix (c).
                var C = _ffInternals.Str(param.Radix, m, c);

                //     viii. Let B = A.
                // This step differs from Encrypt, using B instead of A
                B = A;
                //     ix. Let A = C.
                // This step differs from Encrypt, using A instead of B
                A = C;
            }

            // 7. Return A || B.
            return NumeralString.ToBitString(A)
                .ConcatenateBits(NumeralString.ToBitString(B));
        }
    }
}