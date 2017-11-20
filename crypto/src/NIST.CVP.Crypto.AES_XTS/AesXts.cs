using System;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public class AesXts : IAesXts
    {
        private readonly IAesXtsInternals _internals = new AesXtsInternals();

        public EncryptionResult Encrypt(XtsKey key, BitString plainText, BitString i)
        {
            // 1
            var P = Chunkify(plainText);
            var m = P.Length - 1;
            var C = new BitString[m + 1];

            for (var q = 0; q <= m - 2; q++)
            {
                C[q] = EncryptBlock(key, P[q], i, q);
            }

            // 2
            var b = plainText.BitLength % 128;

            // 3
            if (b == 0)
            {
                C[m - 1] = EncryptBlock(key, P[m - 1], i, m - 1);
                C[m] = new BitString(0);
            }
            // 4
            else
            {
                var CC = EncryptBlock(key, P[m - 1], i, m - 1);
                C[m] = CC.MSBSubstring(0, b);
                var CP = CC.MSBSubstring(b, 128 - b);
                var PP = BitString.ConcatenateBits(P[m], CP);
                C[m - 1] = EncryptBlock(key, PP, i, m);
            }

            // 5
            return new EncryptionResult(BuildResult(C));
        }

        public DecryptionResult Decrypt(XtsKey key, BitString cipherText, BitString i)
        {
            // 1
            var C = Chunkify(cipherText);
            var m = C.Length - 1;
            var P = new BitString[m + 1];

            for (var q = 0; q <= m - 2; q++)
            {
                P[q] = DecryptBlock(key, C[q], i, q);
            }

            // 2
            var b = cipherText.BitLength % 128;

            // 3
            if (b == 0)
            {
                P[m - 1] = DecryptBlock(key, C[m - 1], i, m - 1);
                P[m] = new BitString(0);
            }
            // 4
            else
            {
                var PP = DecryptBlock(key, C[m - 1], i, m);
                P[m] = PP.MSBSubstring(0, b);
                var CP = PP.MSBSubstring(b, 128 - b);
                var CC = BitString.ConcatenateBits(C[m], CP);
                P[m - 1] = DecryptBlock(key, CC, i, m - 1);
            }

            return new DecryptionResult(BuildResult(P));
        }

        public BitString GetIFromInteger(int dataUnitSeqNumber)
        {
            if (dataUnitSeqNumber < 0 || dataUnitSeqNumber > 255)
            {
                throw new ArgumentException("Invalid dataUnitSeqNumber in XTS");
            }

            var bsBytes = new byte[16];

            for (var i = 0; i < 16; i++)
            {
                bsBytes[i] = 0;
            }

            bsBytes[0] = Convert.ToByte(dataUnitSeqNumber);

            return new BitString(bsBytes);
        }

        private BitString EncryptBlock(XtsKey key, BitString plainText, BitString i, int j)
        {
            // 1
            var T = _internals.MultiplyByAlpha(_internals.EncryptEcb(key.Key2, i), j);

            // 2
            var PP = BitString.XOR(plainText, T);

            // 3
            var CC = _internals.EncryptEcb(key.Key1, PP);

            // 4
            var C = BitString.XOR(CC, T);

            return C;
        }

        private BitString DecryptBlock(XtsKey key, BitString cipherText, BitString i, int j)
        {
            // 1
            var T = _internals.MultiplyByAlpha(_internals.EncryptEcb(key.Key2, i), j);

            // 2
            var CC = BitString.XOR(cipherText, T);

            // 3
            var PP = _internals.DecryptEcb(key.Key1, CC);

            // 4
            var P = BitString.XOR(PP, T);

            return P;
        }

        private BitString BuildResult(BitString[] array)
        {
            var result = new BitString(0);
            foreach (var bs in array)
            {
                result = result.ConcatenateBits(bs);
            }

            return result;
        }

        private BitString[] Chunkify(BitString bs)
        {
            var blocks = (bs.BitLength / 128) + 1;
            var result = new BitString[blocks];

            for (var i = 0; i < blocks; i++)
            {
                if (i == blocks - 1)
                {
                    // For the last block, grab all remaining bits
                    result[i] = bs.MSBSubstring(i * 128, bs.BitLength - i * 128);
                }
                else
                {
                    result[i] = bs.MSBSubstring(i * 128, 128);
                }
            }

            return result;
        }
    }
}
