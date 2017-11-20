using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public class AesXtsInternals : IAesXtsInternals
    {
        private readonly IAES_ECB _aes = new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));

        public BitString MultiplyByAlpha(BitString encrypted_i, int j)
        {
            var a_i = encrypted_i.ToBytes();

            for (var i = 0; i < j; i++)
            {
                var a_ip1 = new byte[16];

                // Safe to convert to a byte, will never exceed [0, 255]
                a_ip1[0] = Convert.ToByte((2 * (a_i[0] % 128)) ^ (135 * (a_i[15] / 128)));

                for (var k = 1; k < 16; k++)
                {
                    a_ip1[k] = Convert.ToByte((2 * (a_i[k] % 128)) ^ (a_i[k - 1] / 128));
                }

                a_i = a_ip1;
            }

            return new BitString(a_i);
        }

        public BitString EncryptEcb(BitString key, BitString plainText)
        {
            var encryptResult = _aes.BlockEncrypt(key, plainText);
            if (!encryptResult.Success)
            {
                throw new Exception("Failed to encrypt in ECB in XTS");
            }

            return encryptResult.CipherText;
        }

        public BitString DecryptEcb(BitString key, BitString cipherText)
        {
            var decryptResult = _aes.BlockDecrypt(key, cipherText);
            if (!decryptResult.Success)
            {
                throw new Exception("Failed to decrypt in ECB in XTS");
            }

            return decryptResult.PlainText;
        }
    }
}
