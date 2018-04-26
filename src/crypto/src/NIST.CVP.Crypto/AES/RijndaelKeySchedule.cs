using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NLog;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelKeySchedule : IRijndaelKeySchedule
    {
        public int Rounds { get; }
        public int KeyCount { get; }
        public int BlockCount { get; }
        public string ErrorMessage { get; private set; }
        public byte[,,] Schedule { get; private set; }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        public Dictionary<int, int> BitCountMap = new Dictionary<int, int> {{128, 4}, {192, 6}, {256, 8}};

        public RijndaelKeySchedule(int keySizeInBits, int blockSize, byte[,] keyData)
        {
            KeyCount = GetCount(keySizeInBits);
            BlockCount = GetCount(blockSize);
            if (KeyCount == -1)
            {
                ErrorMessage = $"Invalid key size: {keySizeInBits}";
                return;
            }
            if (BlockCount == -1)
            {
                ErrorMessage = $"Invalid block size: {blockSize}";
                return;
            }
            if (KeyCount >= BlockCount)
            {
                Rounds = KeyCount + 6;
            }
            else
            {
                Rounds = BlockCount + 6;
            }
            PopulateSchedule(keyData);
        }

        private void PopulateSchedule(byte[,] keyData)
        {
            try
            {
                byte[,] tempKeyData = new byte[4, KeyCount];
                Schedule = new byte[Rounds + 1, 4, BlockCount];
                for (int j = 0; j < KeyCount; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        tempKeyData[i, j] = keyData[i, j];
                    }
                }
                int t = 0;
                for (int j = 0; (j < KeyCount) && (t < (Rounds + 1) * BlockCount); j++, t++)
                {
                    for (int i = 0; i < 4; i++)
                        Schedule[t / BlockCount, i, t % BlockCount] = tempKeyData[i, j];
                }

                int rconpointer = 0;
                while (t < (Rounds + 1) * BlockCount)
                {
                    /* calculate new values */
                    for (int i = 0; i < 4; i++)
                    {
                        //XOR
                        tempKeyData[i, 0] ^= RijndaelBoxes.S[tempKeyData[(i + 1) % 4, KeyCount - 1]];
                    }
                    tempKeyData[0, 0] ^= RijndaelBoxes.rcon[rconpointer++];

                    if (KeyCount != 8)
                    {
                        for (int j = 1; j < KeyCount; j++)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tempKeyData[i, j] ^= tempKeyData[i, j - 1];
                            }
                        }
                    }
                    else
                    {
                        for (int j = 1; j < KeyCount / 2; j++)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tempKeyData[i, j] ^= tempKeyData[i, j - 1];
                            }
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            tempKeyData[i, KeyCount / 2] ^= RijndaelBoxes.S[tempKeyData[i, KeyCount / 2 - 1]];
                        }

                        for (int j = KeyCount / 2 + 1; j < KeyCount; j++)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tempKeyData[i, j] ^= tempKeyData[i, j - 1];
                            }
                        }

                    }
                    /* copy values into the schedule*/
                    for (int j = 0; (j < KeyCount) && (t < (Rounds + 1) * BlockCount); j++, t++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Schedule[t / BlockCount, i, t % BlockCount] = tempKeyData[i, j];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                ErrorMessage = ex.Message;
            }
        }

        public int GetCount(int size)
        {
            if (BitCountMap.ContainsKey(size))
            {
                return BitCountMap[size];
            }
            return -1;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
