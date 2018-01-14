using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Crypto.TDES_ECB;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapTdes : KeyWrapBaseTdes
    {
        public static BitString Icv3 = new BitString("A6A6A6A6");
        public KeyWrapTdes(ITDES_ECB tdes) : base(tdes) { }

        public override SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher)
        {
            //1.Let ICV3 = 0xA6A6A6A6
            //2.Let S = ICV3 || P
            //3.Return C = TW(S)

            BitString c = Wrap(key, Icv3.ConcatenateBits(plainText), wrapWithInverseCipher);

            return new SymmetricCipherResult(c);
        }

        public override SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher)
        {

            // 2. Let ICV3 = 0xA6A6A6A6
            // 3. Let S = TW^-1(C)
            BitString S = WrapInverse(key, cipherText, wrappedWithInverseCipher);
            // 4. If MSB32(S) != ICV3 then return FAIL and stop.
            if (!S.GetMostSignificantBits(32).Equals(Icv3))
            {
                return new SymmetricCipherResult("Authentication failure.");
            }
            // 5. Return P = LSB32(n-1)(S)
            //*authenticated = true;
            BitString P = S.GetLeastSignificantBits(S.BitLength - 32);
            return new SymmetricCipherResult(P);
        }


    }
}
