using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.Engines
{
    public class AesEngine : IBlockCipherEngine
    {
        private const int _bitsInByte = 8;

        private static readonly int[] _validKeyLengths = { 128, 192, 256 };

        private IRijndaelKeySchedule _keySchedule;

        private BlockCipherDirections _direction;
        private byte[] _key;
        private bool _useInverseCipher;

        public int BlockSizeBits => 128;

        public void Init(IBlockCipherEngineParameters param)
        {
            if (param == null)
            {
                throw new ArgumentException($"{nameof(param)} is null.");
            }
            if (param.Key == null)
            {
                throw new ArgumentException($"{nameof(param.Key)} is null.");
            }

            var keyLengthInBits = param.Key.Length * _bitsInByte;

            if (!_validKeyLengths.Contains(keyLengthInBits))
            {
                throw new ArgumentOutOfRangeException(nameof(param.Key));
            }

            _direction = param.Direction;
            _key = param.Key;
            _useInverseCipher = param.UseInverseCipherMode;

            PopulateKeySchedule();
        }

        public void ProcessSingleBlock(byte[,] block)
        {
            if (_key == null)
            {
                throw new InvalidOperationException("Engine has not been initialized");
            }

            List<(BlockCipherDirections direction, bool useInverseCipher, Action<byte[,]> action)> workerMappings =
                new List<(BlockCipherDirections direction, bool useInverseCipher, Action<byte[,]> action)>()
                {
                    (BlockCipherDirections.Encrypt, false, Encrypt),
                    (BlockCipherDirections.Encrypt, true, Decrypt),
                    (BlockCipherDirections.Decrypt, false, Decrypt),
                    (BlockCipherDirections.Decrypt, true, Encrypt)
                };

            var action = workerMappings
                .FirstOrDefault(
                    w => w.direction == _direction &&
                         w.useInverseCipher == _useInverseCipher
                )
                .action;

            if (action == null)
            {
                throw new ArgumentException($"Invalid arguments passed into {nameof(ProcessSingleBlock)}.");
            }

            action(block);
        }

        private void Encrypt(byte[,] block)
        {
            var roundKey = Array3D.GetSubArray(_keySchedule.Schedule, 0);
            var blockCount = _keySchedule.BlockCount;

            KeyAddition(block, roundKey, blockCount);

            for (int round = 1; round < _keySchedule.Rounds; round++)
            {
                Substitution(block, RijndaelBoxes.S, blockCount);
                ShiftRow(block, 0, blockCount);
                MixColumn(block, blockCount);
                roundKey = Array3D.GetSubArray(_keySchedule.Schedule, round);
                KeyAddition(block, roundKey, blockCount);
            }

            Substitution(block, RijndaelBoxes.S, blockCount);
            ShiftRow(block, 0, blockCount);

            roundKey = Array3D.GetSubArray(_keySchedule.Schedule, _keySchedule.Rounds);
            KeyAddition(block, roundKey, blockCount);
        }

        private void Decrypt(byte[,] block)
        {
            var roundKey = Array3D.GetSubArray(_keySchedule.Schedule, _keySchedule.Rounds);
            var blockCount = _keySchedule.BlockCount;

            KeyAddition(block, roundKey, blockCount);
            Substitution(block, RijndaelBoxes.Si, blockCount);
            ShiftRow(block, 1, blockCount);

            for (int round = _keySchedule.Rounds - 1; round > 0; round--)
            {
                roundKey = Array3D.GetSubArray(_keySchedule.Schedule, round);
                KeyAddition(block, roundKey, blockCount);
                InvMixColumn(block, blockCount);
                Substitution(block, RijndaelBoxes.Si, blockCount);
                ShiftRow(block, 1, blockCount);
            }

            roundKey = Array3D.GetSubArray(_keySchedule.Schedule, 0);
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
            {
                return RijndaelBoxes.Algotable[(RijndaelBoxes.Logtable[a] + RijndaelBoxes.Logtable[b]) % 255];
            }

            return 0;
        }

        private void PopulateKeySchedule()
        {
            var keyBytes = _key.Length;
            int keyBits = keyBytes * 8;
            var blockedKeyDimension = keyBytes / 4;

            byte[,] k = new byte[4, blockedKeyDimension];

            for (int i = 0; i < keyBits / 8; i++)
            {
                k[i % 4, i / 4] = _key[i];
            }
            _keySchedule = new RijndaelKeySchedule(keyBits, BlockSizeBits, k);
        }
    }
}