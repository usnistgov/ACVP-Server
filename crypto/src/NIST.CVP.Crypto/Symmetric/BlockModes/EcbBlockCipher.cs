using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class EcbBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        private readonly IBlockCipherEngine _engine;

        public EcbBlockCipher(IBlockCipherEngine engine)
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

            // block setup is same between encrypt/decrypt
            ProcessPayload(param, numberOfBlocks, outBuffer);

            return new SymmetricCipherResult(new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength));
        }
        
        private void ProcessPayload(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var block = new byte[4, 4];
            
            for (int i = 0; i < numberOfBlocks; i++)
            {
                //put payload into the block
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
                {
                    for (int t = 0; t < 4; t++)
                    {
                        block[t, j] = param.Payload.ToBytes()[i * 16 + 4 * j + t];
                    }
                }

                _engine.ProcessSingleBlock(block);

                //put processed block into into the out buffer
                for (int j = 0; j < _engine.BlockSizeBits / 32; j++)
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