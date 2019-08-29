using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using System.Linq;
using System.Numerics;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Ffx
{
    public class Ff3_1BlockCipher : FfxBlockCipherBase
    {
        public Ff3_1BlockCipher(IBlockCipherEngine engine, IModeBlockCipherFactory factory, IAesFfInternals ffInternals)
            : base(engine, factory, ffInternals) { }

        protected override int NumberOfRounds => 8;
        protected override BitString Encrypt(IFfxModeBlockCipherParameters param)
        {
            var mode = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);

            var X = NumeralString.ToNumeralString(param.Payload);
            var n = X.Numbers.Length;

            // 1. Let u = ⌈n / 2⌉; v = n – u.
            var u = n.CeilingDivide(2);
            var v = n - u;

            // 2. Let A = X[1..u]; B = X[u + 1..n].
            var A = new NumeralString(X.Numbers.Take(u).ToArray());
            var B = new NumeralString(X.Numbers.Skip(u).Take(v).ToArray());

            // TODO Let TL = T[0..27] || 04 and TR = T[32..55] || T[28..31] || 04 .
            // NOTE this step is the difference between FF3 and FF3-1.
            var zeroPad = new BitString(4);
            var TL = param.Iv.MSBSubstring(0, 28).ConcatenateBits(zeroPad);
            var TR = param.Iv.MSBSubstring(32, 24)
                .ConcatenateBits(param.Iv.MSBSubstring(28, 4)
                    .ConcatenateBits(zeroPad));

            // 4. For i from 0 to 7:
            for (var i = 0; i < NumberOfRounds; i++)
            {
                // i. If i is even, let m = u and W = TR, else let m = v and W = TL.
                //     v. If i is even, let m = u; else, let m = v.
                var m = i % 2 == 0 ? u : v;
                var W = i % 2 == 0 ? TR : TL;

                // ii. Let P = W ⊕ [i] 4 || [NUMradix(REV(B))]12 .
                var numB = new BitString(_ffInternals.Num(param.Radix, _ffInternals.Rev(B)));
                if (numB.BitLength < 12 * BitsInByte)
                {
                    var bitsToPrepend = new BitString(12 * BitsInByte - numB.BitLength);
                    numB = bitsToPrepend.ConcatenateBits(numB);
                }

                var P = W.XOR(new BitString(new byte[3]).ConcatenateBits(((byte)i).ToBitString()))
                    .ConcatenateBits(numB);

                // iii. Let S = REVB(CIPHREVB(K) REVB(P)).
                var S = _ffInternals.RevB(mode.ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        _ffInternals.RevB(param.Key),
                        _ffInternals.RevB(P))).Result);

                // iv. Let y = NUM(S).
                var y = _ffInternals.Num(S);

                // v. Let c = (NUMradix (REV(A)) + y) mod radix m .
                var c = (_ffInternals.Num(param.Radix, _ffInternals.Rev(A)) + y).PosMod(BigInteger.Pow(param.Radix, m));

                // vi. Let C = REV(STRm radix(c)).
                var C = _ffInternals.Rev(_ffInternals.Str(param.Radix, m, c));

                // vii. Let A = B.
                A = B;

                // viii. Let B = C.
                B = C;
            }

            // 5. Return A || B.
            return NumeralString.ToBitString(A)
                .ConcatenateBits(NumeralString.ToBitString(B));
        }

        protected override BitString Decrypt(IFfxModeBlockCipherParameters param)
        {
            var mode = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);

            var X = NumeralString.ToNumeralString(param.Payload);
            var n = X.Numbers.Length;

            // 1. Let u = ⌈n / 2⌉; v = n – u.
            var u = n.CeilingDivide(2);
            var v = n - u;

            // 2. Let A = X[1..u]; B = X[u + 1..n].
            var A = new NumeralString(X.Numbers.Take(u).ToArray());
            var B = new NumeralString(X.Numbers.Skip(u).Take(v).ToArray());

            // TODO Let TL = T[0..27] || 04 and TR = T[32..55] || T[28..31] || 04 .
            // NOTE this step is the difference between FF3 and FF3-1.
            var zeroPad = new BitString(4);
            var TL = param.Iv.MSBSubstring(0, 28).ConcatenateBits(zeroPad);
            var TR = param.Iv.MSBSubstring(32, 24)
                .ConcatenateBits(param.Iv.MSBSubstring(28, 4)
                    .ConcatenateBits(zeroPad));

            // 4. For i from 0 to 7:
            for (var i = NumberOfRounds - 1; i >= 0; i--)
            {
                // i. If i is even, let m = u and W = TR, else let m = v and W = TL.
                //     v. If i is even, let m = u; else, let m = v.
                var m = i % 2 == 0 ? u : v;
                var W = i % 2 == 0 ? TR : TL;

                // ii. Let P = W ⊕ [i] 4 || [NUMradix(REV(B))]12 .
                var numA = new BitString(_ffInternals.Num(param.Radix, _ffInternals.Rev(A)));
                if (numA.BitLength < 12 * BitsInByte)
                {
                    var bitsToPrepend = new BitString(12 * BitsInByte - numA.BitLength);
                    numA = bitsToPrepend.ConcatenateBits(numA);
                }

                var P = W.XOR(new BitString(new byte[3]).ConcatenateBits(((byte)i).ToBitString()))
                    .ConcatenateBits(numA);

                // iii. Let S = REVB(CIPHREVB(K) REVB(P)).
                var S = _ffInternals.RevB(mode.ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        _ffInternals.RevB(param.Key),
                        _ffInternals.RevB(P))).Result);

                // iv. Let y = NUM(S).
                var y = _ffInternals.Num(S);

                // v. Let c = (NUMradix (REV(B)) - y) mod radix m .
                var c = (_ffInternals.Num(param.Radix, _ffInternals.Rev(B)) - y).PosMod(BigInteger.Pow(param.Radix, m));

                // vi. Let C = REV(STRm radix(c)).
                var C = _ffInternals.Rev(_ffInternals.Str(param.Radix, m, c));

                // vii. Let B = A.
                B = A;

                // viii. Let A = C.
                A = C;
            }

            // 5. Return A || B.
            return NumeralString.ToBitString(A)
                .ConcatenateBits(NumeralString.ToBitString(B));
        }
    }
}