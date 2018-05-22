using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public abstract class KeyWrapBaseAes : KeyWrapBase
    {
        protected IAES_ECB _aes;

        public KeyWrapBaseAes(IAES_ECB aes)
        {
            _aes = aes;
        }

        protected override BitString Wrap(BitString K, BitString S, bool wrapWithInverseCipher)
        {
            // 0. Pre-conditions
            int n = S.BitLength / 64;
            if ((n < 3) || (S.BitLength % 64 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(S)} length.");
            }
            int keyLen = K.BitLength;
            if ((keyLen != 128) && (keyLen != 192) && (keyLen != 256))
            {
                throw new ArgumentException($"Invalid {nameof(keyLen)}");
            }

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            int s = 6 * (n - 1);
            // 1.b) Let S1,S2,...,Sn be the semi-blocks s.t. S=S1 || S2 || ... || Sn
            // 1.c) Let A0 = S1
            BitString A = S.GetMostSignificantBits(64);
            // 1.d) For i=2,...,n: let Ri0 = Si
            BitString R2n = S.GetLeastSignificantBits(S.BitLength - 64);

            // 2. Calculate the intermediate values.  For t = 1,...,s update variables
            //    as follows:
            for (int t = 1; t <= s; ++t)
            {
                // a) A^t = MSB64(CIPH_K(A^t-1 || R2^t-1)) xor [t]64
                BitString R2 = R2n.GetMostSignificantBits(64);
                BitString t64 = BitString.To64BitString(t);
                BitString block_t = _aes.BlockEncrypt(K, A.ConcatenateBits(R2), wrapWithInverseCipher).Result;

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
            int n = C.BitLength / 64;
            if ((n < 3) || (C.BitLength % 64 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(C)} length.");
            }
            int aesKeyLength = K.BitLength;
            if ((aesKeyLength != 128) && (aesKeyLength != 192) && (aesKeyLength != 256))
            {
                throw new ArgumentException($"Invalid {nameof(aesKeyLength)}");
            }

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            int s = 6 * (n - 1);
            // 1.b) Let C1,C2,...,Cn be the semi-blocks s.t. C=C1 || C2 || ... || Cn
            // 1.c) Let As = C1
            BitString A = C.GetMostSignificantBits(64);
            // 1.d) For i=2,...,n: let Ri0 = Ci
            BitString R2n = C.GetLeastSignificantBits(C.BitLength - 64);

            // 2. Calculate the intermediate values.  For t = s, s-1, ..., 1,
            //    update the variables as follows:
            for (int t = s; t >= 1; --t)
            {
                // a) A^t-1 = MSB(CIPH^-1K((A^t xor [t]64) || Rn^t))
                BitString t64 = BitString.To64BitString(t);
                BitString Rn = R2n.GetLeastSignificantBits(64);
                BitString block_t = _aes.BlockDecrypt(K, A.XOR(t64).ConcatenateBits(Rn), wrappedWithInverseCipher).Result;
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
