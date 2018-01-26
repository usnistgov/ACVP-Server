using System;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public abstract class KeyWrapBaseTdes : KeyWrapBase
    {
        protected ITDES_ECB _tdes;

        public KeyWrapBaseTdes(ITDES_ECB tdes)
        {
            _tdes = tdes;
        }
        protected override BitString Wrap(BitString K, BitString S, bool wrapWithInverseCipher)
        {
            // 0. Pre-conditions
            int n = S.BitLength / 32;
            if ((n < 3) || (S.BitLength % 32 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(S)} length.");
            }
            int keylen = K.BitLength;
            var K2 = K.GetDeepCopy();
            //if (keylen == 192)
            //{
            //    // Convert to 168 bits because TDES Block encrypt takes 168 bits
            //    K2 = to168BitKey(K);
            //    keylen = K2.BitLength;
            //}
            //else
            //{
            //    K2 = K;
            //}
            //if (keylen != 168)
            //{
            //    throw new ArgumentException($"Invalid {nameof(keylen)}");
            //}

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            int s = 6 * (n - 1);
            // 1.b) Let S1,S2,...,Sn be the semi-blocks s.t. S=S1 || S2 || ... || Sn
            // 1.c) Let A0 = S1
            BitString A = S.GetMostSignificantBits(32);
            // 1.d) For i=2,...,n: let Ri0 = Si
            BitString R2n = S.GetLeastSignificantBits(S.BitLength - 32);

            // 2. Calculate the intermediate values.  For t = 1,...,s update variables
            //    as follows:
            for (int t = 1; t <= s; ++t)
            {
                // a) A^t = MSB32(CIPH_K(A^t-1 || R2^t-1)) xor [t]32
                BitString R2 = R2n.GetMostSignificantBits(32);



                //BitString t32 = to_bitstring32((unsigned long long)t);
                BitString t32 = BitString.To64BitString(t).GetLeastSignificantBits(32); 




                BitString block_t = _tdes.BlockEncrypt(K2, A.ConcatenateBits(R2), wrapWithInverseCipher).Result;
                A = block_t.GetMostSignificantBits(32).XOR(t32);
                // b) For i=2,...,n: Ri^t = Ri+1^t-1
                // c) Rn^t = LSB32(CIPH_K(CIPH_K(A^t-1 || R2^t-1)))
                R2n = R2n.GetLeastSignificantBits(R2n.BitLength - 32).ConcatenateBits(block_t.GetLeastSignificantBits(32));
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
            int n = C.BitLength / 32;
            if ((n < 3) || (C.BitLength % 32 != 0))
            {
                throw new ArgumentException($"Invalid {nameof(C)} length.");
            }
            int keyLength = K.BitLength;
            var K2 = K.GetDeepCopy();
            //if (keyLength == 192)
            //{
            //    // Convert to 168 bits because TDES Block encrypt takes 168 bits
            //    K2 = to168BitKey(K);
            //    keyLength = K2.BitLength;
            //}
            //else
            //{
            //    K2 = K;
            //}
            //if (keyLength != 168)
            //{
            //    throw new ArgumentException($"Invalid {nameof(keyLength)}");
            //}

            // 1. Initialize variables
            // 1.a) Let s = 6(n-1)
            int s = 6 * (n - 1);
            // 1.b) Let C1,C2,...,Cn be the semi-blocks s.t. C=C1 || C2 || ... || Cn
            // 1.c) Let As = C1
            BitString A = C.GetMostSignificantBits(32);
            // 1.d) For i=2,...,n: let Ri0 = Ci
            BitString R2n = C.GetLeastSignificantBits(C.BitLength - 32);


            // 2. Calculate the intermediate values.  For t = s, s-1, ..., 1,
            //    update the variables as follows:
            for (int t = s; t >= 1; --t)
            {
                // a) A^t-1 = MSB(CIPH^-1K((A^t xor [t]32) || Rn^t))
                BitString t32 = BitString.To64BitString(t).GetLeastSignificantBits(32);
                BitString Rn = R2n.GetLeastSignificantBits(32);
                BitString block_t = _tdes.BlockDecrypt(K2, A.XOR(t32).ConcatenateBits(Rn), wrappedWithInverseCipher).Result;
                A = block_t.GetMostSignificantBits(32);
                // b) R2^t-1 = LSB(CIPH^-1K((A^t xor [t]32) || Rn^t))
                // c) For i=2,...,n-1, Ri+1^t-1 = Ri^t
                R2n = block_t.GetLeastSignificantBits(32).ConcatenateBits(R2n.GetMostSignificantBits(R2n.BitLength - 32));
            }

            // 3. Output the results:
            // 3.a) Let S1 = A0
            // 3.b) For i=2,...,n: Si = Ri0
            // 3.c) Return S1 || S2 || ... || Sn
            return A.ConcatenateBits(R2n);
        }

        private BitString to168BitKey(BitString K)
        {
            var Kc = new BitString(168);

            int ikc = 0;
            for (int ik = 0; ik < K.BitLength; ++ik)
            {
                if (ik % 8 != 0)
                {
                    var b = K.Bits[ik];

                    Kc.Set(ikc++, b);
                }
            }

            return Kc;
        }
}

    
}