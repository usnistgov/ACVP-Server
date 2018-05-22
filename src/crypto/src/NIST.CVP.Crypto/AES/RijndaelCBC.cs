using System;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NLog;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelCBC : Rijndael
    {
        public RijndaelCBC(IRijndaelInternals iRijndaelInternals)
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            switch (key.Direction)
            {
                case DirectionValues.Encrypt:
                    Encrypt(cipher, key, plainText, numBlocks, block, outBuffer);
                    break;
                case DirectionValues.Decrypt:
                    Decrypt(cipher, key, plainText, numBlocks, block, outBuffer);
                    break;
            }
        }

        private void Encrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            try
            {
                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] = cipher.IV[t + 4 * j];
                    }
                }

                for (int i = 0; i < numBlocks; i++)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] ^= input[i * 16 + 4 * j + t];
                        }
                    }

                    _iRijndaelInternals.EncryptSingleBlock(block, key);

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            outBuffer[i * 16 + 4 * j + t] = block[t, j];
                        }
                    }
                }

                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        cipher.IV[t + 4 * j] = block[t, j];
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.Message);
                throw;
            }
            
        }

        private void Decrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            try
            {
                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] = input[4 * j + t];
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[4 * j + t] = (byte)(block[t, j] ^ cipher.IV[t + 4 * j]);
                    }
                }

                for (int i = 1; i < numBlocks; i++)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = input[i * 16 + 4 * j + t];
                        }
                    }

                    _iRijndaelInternals.EncryptSingleBlock(block, key); ;

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            outBuffer[i * 16 + 4 * j + t] = (byte)(block[t, j] ^ input[4 * j + t + (i - 1) * 16]);
                        }
                    }
                }

                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        cipher.IV[t + 4 * j] = input[4 * j + t + (numBlocks - 1) * 16];
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.Message);
                throw;
            }
        }

        private static Logger ThisLogger => LogManager.GetLogger("Generate");
    }
}
