using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelOFB : Rijndael
    {
        public RijndaelOFB(IRijndaelInternals iRijndaelInternals) : 
            base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
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
                        outBuffer[i * 16 + 4 * j + t] = (byte) (block[t, j] ^ input[i * 16 + 4 * j + t]);
                    }
                }
            }

            //Update IV for the next call
            for (int j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    cipher.IV[4 * j + t] = block[t, j];
                }
            }
        }
    }
}
