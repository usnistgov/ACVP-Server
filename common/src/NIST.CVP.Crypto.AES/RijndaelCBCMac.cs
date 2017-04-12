namespace NIST.CVP.Crypto.AES
{
    public class RijndaelCBCMac : Rijndael
    {
        public RijndaelCBCMac(IRijndaelInternals iRijndaelInternals) 
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
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
            }

            /*  Update the IV when done for next call  */
            for (int j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    cipher.IV[t + 4 * j] = block[t, j];
                }
            }

            /*  Put the final MAC into the outbuffer  */
            for (int j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    outBuffer[4 * j + t] = block[t, j];
                }
            }
        }
    }
}