using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.Engines
{
    public class TdesEngine : IBlockCipherEngine
    {
        private const int _bitsInByte = 8;
        private static readonly int[] _validKeyLengths = { 64, 128, 192 };

        private BlockCipherDirections _direction;
        // TODO get rid of this enum
        private FunctionValues _function;
        private byte[] _key;
        private bool _useInverseCipher;
        private TDESContext _context;
        
        public int BlockSizeBytes => 8;

        public int BlockSizeBits => BlockSizeBytes * _bitsInByte;

        public BlockCipherEngines CipherEngine => BlockCipherEngines.Tdes;

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
        
        public void ProcessSingleBlock(byte[] payLoad, byte[] outBuffer, int blockIndex)
        {
            if (_key == null)
            {
                throw new InvalidOperationException("Engine has not been initialized");
            }
            if (payLoad.Length % BlockSizeBytes != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(payLoad), "not a modulus of the block size");
            }

            List<(BlockCipherDirections direction, bool useInverseCipher, Action<byte[]> action)> workerMappings =
                new List<(BlockCipherDirections direction, bool useInverseCipher, Action<byte[]> action)>()
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

            var block = GetSingleBlock(payLoad, blockIndex);

            action(block);

            PopulateOutputFromResult(block, outBuffer, blockIndex);
        }

        private void PopulateKeySchedule()
        {
            switch (_direction)
            {
                case BlockCipherDirections.Encrypt:
                    _function = FunctionValues.Encryption;
                    break;
                case BlockCipherDirections.Decrypt:
                    _function = FunctionValues.Decryption;
                    break;
                default:
                    throw new ArgumentException(nameof(_direction));
            }

            var keys = new TDESKeys(new BitString(_key));
            _context = new TDESContext(keys, _function);
        }

        private void Encrypt(byte[] block)
        {
            byte[] interm1 = _context.Schedule[0].Apply(block);
            byte[] interm2 = _context.Schedule[1].Apply(interm1);
            byte[] output = _context.Schedule[2].Apply(interm2);

            Array.Copy(output, block, BlockSizeBytes);
        }

        private void Decrypt(byte[] block)
        {
            byte[] interm1 = _context.Schedule[2].Apply(block);
            byte[] interm2 = _context.Schedule[1].Apply(interm1);
            byte[] output = _context.Schedule[0].Apply(interm2);

            Array.Copy(output, block, BlockSizeBytes);
        }

        private byte[] GetSingleBlock(byte[] payLoad, int blockIndex)
        {
            var block = new byte[BlockSizeBytes];

            Array.Copy(payLoad, blockIndex * BlockSizeBytes, block, 0, BlockSizeBytes);

            return block;
        }
        
        private void PopulateOutputFromResult(byte[] block, byte[] outBuffer, int blockIndex)
        {
            Array.Copy(block, 0, outBuffer, blockIndex * BlockSizeBytes, BlockSizeBytes);
        }
    }
}
