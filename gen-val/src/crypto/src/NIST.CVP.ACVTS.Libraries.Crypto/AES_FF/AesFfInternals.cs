using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_FF
{
    /// <summary>
    /// Internal methods for AES-FFX.
    ///
    /// There is a concept of "numeral strings" (which are separate from <see cref="BitString"/>),
    /// but can be represented as such.
    ///
    /// It may be incorrect, but for the moment I am representing numeral strings as
    /// <see cref="BitString"/> where each number is 32 bits of the <see cref="BitString"/>
    /// </summary>
    public class AesFfInternals : IAesFfInternals
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public AesFfInternals(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public BigInteger Num(int radix, NumeralString x)
        {
            // 1. Let x = 0.
            BigInteger result = 0;

            // 2. For i from 1 to LEN(X), let x = x*radix + X[i].
            for (var i = 0; i <= x.Numbers.Length - 1; i++)
            {
                result = result * radix + x.Numbers[i];
            }

            // 3. Return x.
            return result;
        }

        public BigInteger Num(BitString x)
        {
            // 1. Let x = 0.
            BigInteger result = 0;

            x = new BitString(x.Bits.Reverse());

            // 2. For i from 1 to LEN(X), let x = 2x + X[i].
            for (var i = 0; i <= x.BitLength - 1; i++)
            {
                result = 2 * result + (x.Bits[i] ? 1 : 0);
            }

            // 3. Return x.
            return result;
        }

        public NumeralString Str(int radix, int m, BigInteger x)
        {
            var resultArray = new int[m];

            // 1. For i from 1 to m:
            for (var i = 0; i <= m - 1; i++)
            {
                // i. X[m+1–i] = x mod radix;
                resultArray[m - 1 - i] = (int)(x % radix);

                // ii. x = Floor(x/radix).
                // when working with integer division "toward zero" is the default truncation, so effectively a floor with positive numbers.
                x /= radix;
            }

            // 2. Return X.
            return new NumeralString(resultArray);
        }

        public NumeralString Rev(NumeralString x)
        {
            // 1. For i from 1 to LEN(X), let Y[i] = X[LEN(X)+1–i].
            var result = new int[x.Numbers.Length];
            for (var i = 0; i <= x.Numbers.Length - 1; i++)
            {
                result[i] = x.Numbers[x.Numbers.Length - 1 - i];
            }

            // 2. Return Y[1..LEN(X)].
            return new NumeralString(result);
        }

        public BitString RevB(BitString x)
        {
            // 1. For i from 0 to BYTELEN(X)–1 and j from 1 to 8, let Y[8i+j] = X[8×(BYTELEN(X)–1–i)+j].
            // 2. Return Y[1..8×BYTELEN(X)].
            return new BitString(MsbLsbConversionHelpers.ReverseByteOrder(x.ToBytes()));
        }

        public BitString Prf(BitString x, BitString key)
        {
            // This method runs cbc mac on the full block string against the key
            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            var mode = _modeFactory.GetStandardCipher(engine, BlockCipherModesOfOperation.CbcMac);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(engine.BlockSizeBits),
                key,
                x
            );

            return mode.ProcessPayload(param).Result;
        }
    }
}
