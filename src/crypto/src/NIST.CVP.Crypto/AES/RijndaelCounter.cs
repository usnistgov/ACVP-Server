using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelCounter : Rijndael
    {
        public RijndaelCounter(IRijndaelInternals iRijndaelInternals) 
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            BitString IV = cipher.IV.GetDeepCopy();

            for (int i = 0; i < numBlocks; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = IV[t + 4 * j] = cipher.IV[t + 4 * j];
                        }
                    }
                }
                else
                {
                    BumpCount(IV, cipher.BlockLength);
                    for (int j = 0; j < cipher.BlockLength / 32; j++)
                    {
                        for (int t = 0; t < 4; t++)
                        {
                            block[t, j] = IV[t + 4 * j];
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

            /*  Update IV for the next call  */
            BumpCount(IV, cipher.BlockLength);
            for (int j = 0; j < cipher.BlockLength / 8; j++)
            {
                cipher.IV[j] = IV[j];
            }
        }

        private void BumpCount(BitString iv, int cipherBlockLength)
        {
            for (int i = cipherBlockLength / 8 - 1; i >= 0; i--)
            {
                if (iv[i] == 0xff)
                {
                    iv[i] = 0x00;
                }
                else
                {
                    iv[i]++;
                    return;
                }
            }
        }
    }
}