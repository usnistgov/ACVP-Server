using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class OfbiBlockCipher : ModeBlockCipherBase<SymmetricCipherWithIvResult>
    {
        private const int PARTITIONS = 3;

        public override bool IsPartialBlockAllowed => true;

        public OfbiBlockCipher(IBlockCipherEngine engine) : base(engine)
        {
            if (engine.BlockSizeBits != 64)
            {
                throw new NotSupportedException("Mode valid only with TDES Engine");
            }
        }

        public override SymmetricCipherWithIvResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();
            var actualBitsToProcess = param.Payload.BitLength;
            param.Payload = BitString.PadToModulus(param.Payload, _engine.BlockSizeBits);

            // OFB always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);


            if (numberOfBlocks < PARTITIONS)
            {
                throw new ArgumentOutOfRangeException(nameof(param.Payload), $"Ofb-I mode requires a minimum of {PARTITIONS} blocks");
            }

            var ivs = SetupIvs(param.Iv);
            ProcessPayload(param, ivs, numberOfBlocks, outBuffer);

            return new SymmetricCipherWithIvResult(
                new BitString(outBuffer).GetMostSignificantBits(actualBitsToProcess),
                ivs
            );
        }

        private BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new[]
            {
                iv,
                iv.AddWithModulo(new BitString("5555555555555555"), _engine.BlockSizeBits),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), _engine.BlockSizeBits)
            };
        }

        private void ProcessPayload(IModeBlockCipherParameters param, BitString[] ivs, int numberOfBlocks, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();
            var iv = param.Iv.GetDeepCopy().ToBytes();
            var ivOutBuffer = new byte[iv.Length];

            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                _engine.ProcessSingleBlock(iv, ivOutBuffer, 0);

                // XOR processed IV onto current block payload
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] =
                        (byte)(payLoad[i * _engine.BlockSizeBytes + j] ^ ivOutBuffer[j]);
                }

                // track current ivOutBuffer to use as next i % 3 iv input.
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    ivs[i % PARTITIONS][j] = ivOutBuffer[j];
                }
                // the next round IV to use
                iv = ivs[(i + 1) % PARTITIONS].ToBytes();
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}
