using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public class RijndaelInternals : IRijndaelInternals
    {
        public virtual void EncryptSingleBlock(byte[,] block, Key key)
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

        public virtual void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i, j] ^= roundKey[i, j];
                }
            }
        }

        public virtual void Substitution(byte[,] block, byte[] box, int blockCount)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i, j] = box[block[i, j]];
                }
            }
        }

        public virtual void ShiftRow(byte[,] block, int d, int blockCount)
        {
            byte[] tmp = new byte[blockCount];
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    var shift = ((blockCount - 4) >> 1);
                    tmp[j] = block[i, (j + RijndaelBoxes.shifts[shift, i, d]) % blockCount];
                }

                for (int j = 0; j < blockCount; j++)
                {
                    block[i, j] = tmp[j];
                }
            }
        }

        public virtual void MixColumn(byte[,] block, int blockCount)
        {
            byte[,] tmp = new byte[4, blockCount];
            for (byte j = 0; j < blockCount; j++)
            {
                for (byte i = 0; i < 4; i++)
                {
                    tmp[i, j] = (byte)(
                        Multiply(2, block[i, j]) ^
                        Multiply(3, block[(i + 1) % 4, j]) ^
                        block[(i + 2) % 4, j] ^
                        block[(i + 3) % 4, j]
                    );
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < blockCount; j++)
                {
                    block[i, j] = tmp[i, j];
                }

            }
        }

        public virtual byte Multiply(byte a, byte b)
        {
            if (a > 0 && b > 0)
                return RijndaelBoxes.Algotable[(RijndaelBoxes.Logtable[a] + RijndaelBoxes.Logtable[b]) % 255];
            else
                return 0;
        }
    }
}
