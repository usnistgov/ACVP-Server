using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelInternals : IRijndaelInternals
    {
        public virtual void EncryptSingleBlock(byte[,] block, Key key)
        {
            List<(DirectionValues direction, bool useInverseCipher, Action<byte[,], Key> action)> workerMappings =
                new List<(DirectionValues direction, bool useInverseCipher, Action<byte[,], Key> action)>()
            {
                (DirectionValues.Encrypt, false, Encrypt),
                (DirectionValues.Encrypt, true, Decrypt),
                (DirectionValues.Decrypt, false, Decrypt),
                (DirectionValues.Decrypt, true, Encrypt)
            };
            
            var action = workerMappings
                .FirstOrDefault(
                    w => w.direction == key.Direction && 
                        w.useInverseCipher == key.UseInverseCipher
                )
                .action;

            if (action == null)
            {
                throw new ArgumentException("Invalid arguments passed into EncryptSingleBlock.");
            }

            action(block, key);
        }

        private void Encrypt(byte[,] block, Key key)
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

        private void Decrypt(byte[,] block, Key key)
        {
            var roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, key.KeySchedule.Rounds);
            var blockCount = key.KeySchedule.BlockCount;

            KeyAddition(block, roundKey, blockCount);
            Substitution(block, RijndaelBoxes.Si, blockCount);
            ShiftRow(block, 1, blockCount);

            for (int round = key.KeySchedule.Rounds-1; round > 0; round--)
            {
                roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, round);
                KeyAddition(block, roundKey, blockCount);
                InvMixColumn(block, blockCount);
                Substitution(block, RijndaelBoxes.Si, blockCount);
                ShiftRow(block, 1, blockCount);
            }

            roundKey = Array3D.GetSubArray(key.KeySchedule.Schedule, 0);
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

        public virtual void InvMixColumn(byte[,] block, int blockCount)
        {
            byte[,] tmp = new byte[4, blockCount];
            for (byte j = 0; j < blockCount; j++)
            {
                for (byte i = 0; i < 4; i++)
                {
                    tmp[i, j] = (byte)(
                        Multiply(14, block[i, j]) ^
                        Multiply(11, block[(i + 1) % 4, j]) ^
                        Multiply(13, block[(i + 2) % 4, j]) ^
                        Multiply(9, block[(i + 3) % 4, j])
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
