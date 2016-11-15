using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES
{
    public abstract class Rijndael
    {
        public const int MAXKC = 8;
        public const int MAX_IV_SIZE_BYTES = 16;
        public Key MakeKey(byte[] keyData, DirectionValues direction)
        {
            var key = new Key {BlockLength = 128, Bytes = keyData};
            byte[,] k = new byte[4, MAXKC];
            int keyLen = keyData.Length * 8;
            for (int i = 0; i < keyLen/8; i++)
            {
                k[i % 4, i / 4] = keyData[i];
            }
            key.KeySchedule = new RijndaelKeySchedule(keyData.Length * 8, key.BlockLength, k);
            return key;
        }

        public BitString BlockEncrypt(Cipher cipher, Key key, byte[] plainText, int outputLengthInBits)
        {
            int numBlocks= plainText.Length * 8 / cipher.BlockLength;
            byte[,] block = new byte[4,8];
            byte[] outBuffer = new byte[outputLengthInBits/8];
           
            BlockEncryptWorker(cipher, key, plainText, numBlocks, block, outBuffer);
            return new BitString(outBuffer);
        }

        protected abstract void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block,
            byte[] outBuffer);
       


        protected void EncryptSingleBlock(byte[,] block, Key key)
        {
            var roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, 0);
            var blockCount = key.KeySchedule.BlockCount;
            KeyAddition(block, roundKey, blockCount);
            for (int round = 1; round < key.KeySchedule.Rounds; round++)
            {
                Substitution(block, RijndaelBoxes.S, blockCount);
                ShiftRow(block, 0, blockCount);
                MixColumn(block, blockCount);
                roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, round);
                KeyAddition(block, roundKey, blockCount);
            }
            Substitution(block, RijndaelBoxes.S, blockCount);
            ShiftRow(block, 0, blockCount);
            roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, key.KeySchedule.Rounds);
            KeyAddition(block, roundKey, blockCount);
        }

        /* Xor corresponding text input and round key input bytes */
        protected void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i,j] ^= roundKey[i,j];
                }   
            }   
        }

        /* Replace every byte of the input by the byte at that place in the nonlinear S-box */
        protected void Substitution(byte[,] block, byte[]box, int blockCount)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i,j] = box[block[i,j]];
                }             
            }
        }

        /* Row 0 remains unchanged, The other three rows are shifted a variable amount*/
        protected void ShiftRow(byte[,] block, int d, int blockCount)
        {
            byte[] tmp = new byte[blockCount];
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    var shift = ((blockCount - 4) >> 1);
                    tmp[j] = block[i,(j + RijndaelBoxes.shifts[shift,i,d]) % blockCount];
                }

                for (int j = 0; j < blockCount; j++)
                {
                    block[i,j] = tmp[j];
                }              
            }
        }

        /* Mix the four bytes of every column in a linear way */
        protected void MixColumn(byte[,] block, int blockCount)
        {
            byte[,] tmp = new byte[4,blockCount];
            for (byte j = 0; j < blockCount; j++)
            {
                for (byte i = 0; i < 4; i++)
                {
                    tmp[i,j] =(byte) (Multiply(2, block[i, j]) ^ Multiply(3, block[(i + 1) % 4, j]) ^ block[(i + 2) % 4,j] ^ block[(i + 3) % 4,j]);
                }      
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i,j] = tmp[i,j];
                }
          
            }
	    }

        protected byte Multiply(byte a, byte b)
        {
            if (a > 0 && b > 0)
                return RijndaelBoxes.Algotable[(RijndaelBoxes.Logtable[a] + RijndaelBoxes.Logtable[b]) % 255];
            else
                return 0;
        }
    }
}
