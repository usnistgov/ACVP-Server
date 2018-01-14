using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapWithPaddingAes : KeyWrapBaseAes
    {
        public static BitString Icv2 = new BitString("A65959A6");

        public KeyWrapWithPaddingAes(IAES_ECB aes) : base(aes) { }

        public override SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher)
        {
            // 1. Let ICV2 = 0xA65959A6
            // 2. Let padlen = 8 * ceil(len(P)/64) - len(P)/8
            var padLen = 8 * (int) System.Math.Ceiling(plainText.BitLength / 64.0) - (plainText.BitLength / 8);

            // 3. Let PAD = 0^8*padlen
            var pad = BitString.Zeroes(8 * padLen);

            // 4. Let S = ICV2 || [len(P)/8] under 2^32 || P || PAD
            var S = new BitString(0);
            S = S.ConcatenateBits(Icv2);
            S = S.ConcatenateBits(BitString.To32BitString(plainText.BitLength / 8));
            S = S.ConcatenateBits(plainText);
            S = S.ConcatenateBits(pad);

            if (plainText.BitLength <= 64)
            {
                var aesResult = _aes.BlockEncrypt(key, S, wrapWithInverseCipher);
                return aesResult.Success ? new SymmetricCipherResult(aesResult.Result) : new SymmetricCipherResult(aesResult.ErrorMessage);
            }
            else
            {
                var c = Wrap(key, S, wrapWithInverseCipher);
                return new SymmetricCipherResult(c);
            }
        }

        public override SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher)
        {
            // 0. Check for valid bitlength ciphertext
            if (cipherText.BitLength < 128 || cipherText.BitLength % 64 != 0)
            {
                return new SymmetricCipherResult("Invalid length for ciphertext");
            }
            
            // 1. Let n = number of semi-blocks in C (number of half-blocks, where a block is 128-bits)
            var n = cipherText.BitLength / 64;

            // 2. Let ICV2 = 0xA65959A6
            // 3. Set S based on semi-blocks
            var S = new BitString(32);
            if (n == 2)
            {
                var aesResult =  _aes.BlockDecrypt(key, cipherText, wrappedWithInverseCipher);
                if (!aesResult.Success)
                {
                    return new SymmetricCipherResult(aesResult.ErrorMessage);
                }

                S = aesResult.Result;
            }
            else if (n > 2)
            {
                S = WrapInverse(key, cipherText, wrappedWithInverseCipher);
            }

            // 4. Check MSB(S, 32) to be equal to ICV2
            if (!S.MSBSubstring(0, 32).Equals(Icv2))
            {
                return new SymmetricCipherResult("ICV2 not found as most significant bits of S");
            }

            // 5. pLen = int(LSB(MSB(S, 64), 32))
            var pLen = (int)S.MSBSubstring(0, 64).Substring(0, 32).ToPositiveBigInteger();

            // 6. padLen = 8 * (n - 1) - pLen
            var padLen = 8 * (n - 1) - pLen;

            // 7. Make sure 0 < padLen < 7
            if (padLen < 0 || padLen > 7)
            {
                return new SymmetricCipherResult("Invalid padLen");
            }

            // 8. If the padding isn't 0s, then fail
            if (!S.Substring(0, 8 * padLen).Equals(BitString.Zeroes(8 * padLen)))
            {
                return new SymmetricCipherResult("Padding is not all 0s");
            }

            // 9. Return p = MSB(LSB(S, 64*(n-1)), 8*pLen)
            var p = S.Substring(0, 64 * (n - 1)).MSBSubstring(0, 8 * pLen);
            return new SymmetricCipherResult(p);
        }
    }
}
