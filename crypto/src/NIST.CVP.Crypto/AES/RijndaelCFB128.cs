using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelCFB128 : Rijndael
    {
        public RijndaelCFB128(IRijndaelInternals iRijndaelInternals) 
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            switch (key.Direction)
            {
                case DirectionValues.Encrypt:
                    Encrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
                case DirectionValues.Decrypt:
                    // CFB128 contains different logic for setting up the block for decrypt, but uses the internal encrypt operation.
                    key.Direction = DirectionValues.Encrypt;
                    Decrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
            }
        }
        
        private void Encrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = cipher.IV[t + 4 * j];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[i * 16 + 4 * j + t] = block[t, j] ^= input[i * 16 + 4 * j + t];
                    }
                }
            }

            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    cipher.IV[4 * j + t] = block[t, j];
                }
            }
        }

        private void Decrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = cipher.IV[t + 4 * j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = input[(i - 1) * 16 + 4 * j + t];
                        }
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);

                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[i * 16 + 4 * j + t] = (byte) (input[i * 16 + 4 * j + t] ^ block[t, j]);
                    }
                }
            }

            /*  Update IV for the next call  */
            for (int j = 0; j < cipher.BlockLength / 8; j++)
            {
                cipher.IV[j] = input[(numBlocks - 1) * 16 + j];
            }
        }
    }
}