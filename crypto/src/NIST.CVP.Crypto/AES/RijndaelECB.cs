using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelECB : Rijndael
    {
        public RijndaelECB(IRijndaelInternals iRijndaelInternals)
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            
            for (int i = 0; i < numBlocks; i++)
            {
                //put plaintext into the block
                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] = plainText[i * 16 + 4 * j + t];
                    }
                }

                EncryptSingleBlock(block, key);

                //put encrypted block into into the cipher text
                for (int j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[i * 16 + 4 * j + t] = block[t, j];
                    }
                }
            }
            
        }
    }
}
