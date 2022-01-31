using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.ShiftRegister
{
    public class ShiftRegisterStrategyFullBlock : IShiftRegisterStrategy
    {
        private readonly IBlockCipherEngine _engine;

        public ShiftRegisterStrategyFullBlock(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public int ShiftSize => _engine.BlockSizeBits;

        public void SetSegmentInProcessedBlock(byte[] payload, int segmentNumber, byte[] ivOutBuffer)
        {
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                ivOutBuffer[i] ^= payload[segmentNumber * _engine.BlockSizeBytes + i];
            }
        }

        public void SetOutBufferSegmentFromIvXorPayload(byte[] block, int segmentNumber, byte[] outBuffer)
        {
            var outBufferIndex = segmentNumber * _engine.BlockSizeBytes;
            var blockIndex = 0;
            for (int i = outBufferIndex; i < outBufferIndex + _engine.BlockSizeBytes; i++)
            {
                outBuffer[i] = block[blockIndex];
                blockIndex++;
            }
        }

        public void SetNextRoundIv(byte[] block, int segmentNumber, byte[] nextRoundIv)
        {
            Array.Copy(block, segmentNumber * _engine.BlockSizeBytes, nextRoundIv, 0, nextRoundIv.Length);
        }
    }
}
