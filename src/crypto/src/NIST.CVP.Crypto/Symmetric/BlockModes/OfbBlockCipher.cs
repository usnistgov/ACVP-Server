using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class OfbBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        public override bool IsPartialBlockAllowed => true;

        public OfbBlockCipher(IBlockCipherEngine engine) : base(engine) { }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();

            // OFB always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            ProcessPayload(param, numberOfBlocks, outBuffer);
            
            return new SymmetricCipherResult(
                new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength)
            );
        }

        private void ProcessPayload(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            // Concatenate payload to block boundary
            var bitsInLastBlock = param.Payload.BitLength % _engine.BlockSizeBits;
            var payLoad = param.Payload.GetDeepCopy().ConcatenateBits(BitString.Zeroes(_engine.BlockSizeBits - bitsInLastBlock)).ToBytes();

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
                        (byte) (payLoad[i * _engine.BlockSizeBytes + j] ^ ivOutBuffer[j]);
                }

                // update next block IV to the processed IV of this block
                Array.Copy(ivOutBuffer, iv, iv.Length);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}