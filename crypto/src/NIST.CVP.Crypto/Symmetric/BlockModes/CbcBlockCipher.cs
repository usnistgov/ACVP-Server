using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class CbcBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        private readonly IBlockCipherEngine _engine;

        public CbcBlockCipher(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(_engine.BlockSizeBits, param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            if (param.Direction == BlockCipherDirections.Encrypt)
            {
                Encrypt(param, numberOfBlocks, outBuffer);
            }
            else
            {
                Decrypt(param, numberOfBlocks, outBuffer);
            }
            
            return new SymmetricCipherResult(
                new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength)
            );
        }

        private void Encrypt(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var block = new byte[4, 4];

            // put the IV into the block
            for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    block[t, j] = param.Iv[t + 4 * j];
                }
            }

            // for each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                // XOR payload onto block
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] ^= param.Payload[i * 16 + 4 * j + t];
                    }
                }

                _engine.ProcessSingleBlock(block);

                // put processed block into out buffer
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[i * 16 + 4 * j + t] = block[t, j];
                    }
                }
            }

            // Update the Iv for the next call
            for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    param.Iv[t + 4 * j] = block[t, j];
                }
            }
        }

        private void Decrypt(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var block = new byte[4, 4];

            // put the block's worth of payload into a block
            for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    block[t, j] = param.Payload[4 * j + t];
                }
            }

            _engine.ProcessSingleBlock(block);

            // XOR the first processed block with the IV
            for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    outBuffer[4 * j + t] = (byte)(block[t, j] ^ param.Iv[t + 4 * j]);
                }
            }

            // For each remaining block
            for (int i = 1; i < numberOfBlocks; i++)
            {
                // put into a block
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] = param.Payload[i * 16 + 4 * j + t];
                    }
                }

                _engine.ProcessSingleBlock(block); ;

                // update the outbuffer with the XOR of the processed block and the payload
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        outBuffer[i * 16 + 4 * j + t] = (byte)(block[t, j] ^ param.Payload[4 * j + t + (i - 1) * 16]);
                    }
                }
            }

            // Update the Iv for the next call
            for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
            {
                for (int t = 0; t < 4; t++)
                {
                    param.Iv[t + 4 * j] = param.Payload[4 * j + t + (numberOfBlocks - 1) * 16];
                }
            }
        }
    }
}