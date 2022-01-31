using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap
{
    public abstract class KeyWrapBaseAes : KeyWrapBase
    {
        protected readonly IModeBlockCipher<SymmetricCipherResult> Cipher;

        protected KeyWrapBaseAes(IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory)
        {
            Cipher = cipherFactory.GetStandardCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), BlockCipherModesOfOperation.Ecb);
        }

        protected override BitString Wrap(BitString K, BitString S, bool wrapWithInverseCipher)
        {
            // 0. Pre-conditions
            var n = S.BitLength / 64;
            if ((n < 3) || (S.BitLength % 64 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(S)} length.");
            }

            var keyLen = K.BitLength;
            if ((keyLen != 128) && (keyLen != 192) && (keyLen != 256))
            {
                throw new ArgumentException($"Invalid {nameof(keyLen)}");
            }

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            var s = 6 * (n - 1);
            // 1.b) Let S1,S2,...,Sn be the semi-blocks s.t. S=S1 || S2 || ... || Sn
            // 1.c) Let A0 = S1
            var A = S.GetMostSignificantBits(64);
            // 1.d) For i=2,...,n: let Ri0 = Si
            var R2n = S.GetLeastSignificantBits(S.BitLength - 64);

            // 2. Calculate the intermediate values.  For t = 1,...,s update variables
            //    as follows:
            for (var t = 1; t <= s; ++t)
            {
                // a) A^t = MSB64(CIPH_K(A^t-1 || R2^t-1)) xor [t]64
                var R2 = R2n.GetMostSignificantBits(64);
                var t64 = BitString.To64BitString(t);
                var block_t = Cipher.ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        K,
                        A.ConcatenateBits(R2),
                        wrapWithInverseCipher
                    )).Result;

                A = block_t.GetMostSignificantBits(64).XOR(t64);
                // b) For i=2,...,n: Ri^t = Ri+1^t-1
                // c) Rn^t = LSB64(CIPH_K(CIPH_K(A^t-1 || R2^t-1)))
                R2n = R2n
                    .GetLeastSignificantBits(R2n.BitLength - 64)
                    .ConcatenateBits(block_t.GetLeastSignificantBits(64));
            }

            // 3. Output the results:
            // a) Let C1 = A^s
            // b) For i=2,...,n: Ci = Ri^s
            // c) Return C1 || C2 || ... || Cn
            return A.ConcatenateBits(R2n);
        }

        protected override BitString WrapInverse(BitString K, BitString C, bool wrappedWithInverseCipher)
        {
            // 0. Pre-conditions
            var n = C.BitLength / 64;
            if ((n < 3) || (C.BitLength % 64 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(C)} length.");
            }

            var aesKeyLength = K.BitLength;
            if ((aesKeyLength != 128) && (aesKeyLength != 192) && (aesKeyLength != 256))
            {
                throw new ArgumentException($"Invalid {nameof(aesKeyLength)}");
            }

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            var s = 6 * (n - 1);
            // 1.b) Let C1,C2,...,Cn be the semi-blocks s.t. C=C1 || C2 || ... || Cn
            // 1.c) Let As = C1
            var A = C.GetMostSignificantBits(64);
            // 1.d) For i=2,...,n: let Ri0 = Ci
            var R2n = C.GetLeastSignificantBits(C.BitLength - 64);

            // 2. Calculate the intermediate values.  For t = s, s-1, ..., 1,
            //    update the variables as follows:
            for (var t = s; t >= 1; --t)
            {
                // a) A^t-1 = MSB(CIPH^-1K((A^t xor [t]64) || Rn^t))
                var t64 = BitString.To64BitString(t);
                var Rn = R2n.GetLeastSignificantBits(64);
                var block_t = Cipher.ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        K,
                        A.XOR(t64).ConcatenateBits(Rn),
                        wrappedWithInverseCipher
                    )).Result;

                A = block_t.GetMostSignificantBits(64);
                // b) R2^t-1 = LSB(CIPH^-1K((A^t xor [t]64) || Rn^t))
                // c) For i=2,...,n-1, Ri+1^t-1 = Ri^t
                R2n = block_t.GetLeastSignificantBits(64).ConcatenateBits(R2n.GetMostSignificantBits(R2n.BitLength - 64));
            }

            // 3. Output the results:
            // 3.a) Let S1 = A0
            // 3.b) For i=2,...,n: Si = Ri0
            // 3.c) Return S1 || S2 || ... || Sn
            return A.ConcatenateBits(R2n);
        }
    }
}
