using System;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister
{
    public class ShiftRegisterStrategyByte : IShiftRegisterStrategy
    {
        private readonly IBlockCipherEngine _engine;

        public ShiftRegisterStrategyByte(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public int ShiftSize => 8;

        public void SetSegmentInProcessedBlock(byte[] payload, int segmentNumber, byte[] ivOutBuffer)
        {
            // XOR the current segment number payload byte onto the first byte of the ivOutBuffer
            ivOutBuffer[0] =
                (byte) (payload[segmentNumber] ^ ivOutBuffer[0]);
        }

        public void SetOutBufferSegmentFromIvXorPayload(byte[] block, int segmentNumber, byte[] outBuffer)
        {
            // Set the outBuffer segment equal to the first segment of the block
            outBuffer[segmentNumber] = block[0];
        }

        public void SetNextRoundIv(byte[] block, int segmentNumber, byte[] iv)
        {
            // next round IV is the current round IV shifted left, then appended with the current segment
            for (int i = 0; i < iv.Length - 1; i++)
            {
                iv[i] = iv[i + 1];
            }

            if (segmentNumber == 0)
            {
                iv[iv.Length - 1] = block[0];
            }
            else
            {
                iv[iv.Length - 1] = block[segmentNumber];
            }
        }
    }
}
