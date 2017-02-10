using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES
{
    public class RijndaelCFB8 : Rijndael
    {
        public RijndaelCFB8(IRijndaelInternals iRijndaelInternals) 
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            switch (key.Direction)
            {
                case DirectionValues.Encrypt:
                    Encrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
                case DirectionValues.Decrypt:
                    // CFB1 contains different logic for setting up the block for decrypt, but uses the internal encrypt operation.
                    key.Direction = DirectionValues.Encrypt;
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
                            iv[t + 4 * j] = block[t, j] = cipher.IV[t + 4 * j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
                    {
                        iv[j] = iv[j + 1];
                    }
                    iv[cipher.BlockLength / 8 - 1] = block[0, 0];

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = iv[t + 4 * j];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                block[0, 0] ^= input[i];
                outBuffer[i] = block[0, 0];
            }

            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
                cipher.IV[j] = iv[j + 1];
            cipher.IV[cipher.BlockLength / 8 - 1] = block[0, 0];
        }

        private void Decrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            BitString IV = cipher.IV.GetDeepCopy();

            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            IV[t + 4 * j] = block[t, j] = cipher.IV[t + 4 * j];
                        }
                    }
                else
                {
                    for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
                    {
                        IV[j] = IV[j + 1];
                    }
                    IV[cipher.BlockLength / 8 - 1] = input[i - 1];

                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = IV[t + 4 * j];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                outBuffer[i] = (byte) (block[0,0] ^ input[i]);
            }

            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 8 - 1; j++)
            {
                cipher.IV[j] = IV[j + 1];
            }
            cipher.IV[cipher.BlockLength / 8 - 1] = input[numBlocks - 1];
        }
    }
}