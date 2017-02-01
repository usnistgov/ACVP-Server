using System;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES
{
    public class RijndaelCFB1 : Rijndael
    {
        public RijndaelCFB1(IRijndaelInternals iRijndaelInternals) 
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            switch (key.Direction)
            {
                case DirectionValues.Encrypt:
                    Encrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
                case DirectionValues.Decrypt:
                    Decrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
            }
        }

        private void Encrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            BitString iv = cipher.IV.GetDeepCopy();

            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            iv[t + 4 * j] = block[t,j] = cipher.IV[t + 4 * j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
                    {
                        iv[j] = (byte) (((iv[j] << 1) & 0xfe) | ((iv[j + 1] >> 7) & 0x01));
                    }
                    iv[cipher.BlockLength / 8 - 1] =
                        (byte) (((iv[cipher.BlockLength / 8 - 1] << 1) & 0xfe) | ((block[0,0] >> 7) & 0x01));

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t,j] = iv[t + 4 * j];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                var inbit = GetBit(input[i / 8], i);

                block[0,0] ^= (byte) (inbit << 7);
                PutBit(ref (outBuffer[i / 8]), block[0,0], i);
            }

            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
            {
                cipher.IV[j] = (byte) (((iv[j] << 1) & 0xfe) | ((iv[j + 1] >> 7) & 0x01));
            }
            cipher.IV[cipher.BlockLength / 8 - 1] =
                (byte) (((iv[cipher.BlockLength / 8 - 1] << 1) & 0xfe) | ((block[0,0] >> 7) & 0x01));
        }

        private void Decrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            BitString iv = cipher.IV.GetDeepCopy();
            byte inbit;

            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            iv[t + 4 * j] = block[t, j] = cipher.IV[t + 4 * j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
                    {
                        iv[j] = (byte) (((iv[j] << 1) & 0xfe) | ((iv[j + 1] >> 7) & 0x01));
                    }
                    inbit = GetBit(input[(i - 1) / 8], i - 1);
                    iv[cipher.BlockLength / 8 - 1] =
                        (byte) (((iv[cipher.BlockLength / 8 - 1] << 1) & 0xfe) | inbit);

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t,j] = iv[t + 4 * j];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                inbit = GetBit(input[i / 8], i);

                block[0,0] ^= (byte) (inbit << 7);
                PutBit(ref (outBuffer[i / 8]), block[0,0], i);
            }
            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
            {
                cipher.IV[j] = (byte) (((iv[j] << 1) & 0xfe) | ((iv[j + 1] >> 7) & 0x01));
            }
            inbit = GetBit(input[(numBlocks - 1) / 8], numBlocks - 1);
            cipher.IV[cipher.BlockLength / 8 - 1] =
                (byte) (((iv[cipher.BlockLength / 8 - 1] << 1) & 0xfe) | inbit);
        }

        private byte GetBit(byte ch, int i)
        {
            byte mask = 0x01;

            mask <<= 7 - (i % 8);

            if ((byte) (ch & mask) == mask)
            {
                return mask;
            }
            
            return 0;
        }

        void PutBit(ref byte outByte, byte data, int i)
        {
            byte mask = 0x01;

            mask <<= 7 - (i % 8);

            if ((byte) (data & 0x80) == 0x80)
            {
                outByte |= mask;
            }
            else
            {
                outByte &= (byte) ~mask;
            }
        }
    }
}