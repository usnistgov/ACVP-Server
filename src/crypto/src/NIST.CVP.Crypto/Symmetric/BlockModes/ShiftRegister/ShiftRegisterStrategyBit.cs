using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister
{
    public class ShiftRegisterStrategyBit : IShiftRegisterStrategy
    {
        private const int _bitsInByte = 8;
        private readonly IBlockCipherEngine _engine;
        
        public ShiftRegisterStrategyBit(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public int ShiftSize => 1;

        public void SetSegmentInProcessedBlock(byte[] payload, int segmentNumber, byte[] ivOutBuffer)
        {
            var inbit = GetBit(payload[segmentNumber / _bitsInByte], segmentNumber);

            // XOR the current segment number payload bit onto the first byte of the ivOutBuffer
            ivOutBuffer[0] ^= (byte)(inbit << 7);
        }

        public void SetOutBufferSegmentFromIvXorPayload(byte[] block, int segmentNumber, byte[] outBuffer)
        {
            // Set the outBuffer segment equal to the first segment of the block
            PutBit(ref (outBuffer[segmentNumber / _bitsInByte]), block[0], segmentNumber);
        }

        public void SetNextRoundIv(byte[] block, int segmentNumber, byte[] iv)
        {
            // next round IV is the current round IV shifted left, then appended with the current segment
            for (int i = 0; i < iv.Length - 1; i++)
            {
                iv[i] = (byte)(((iv[i] << 1) & 0xfe) | ((iv[i + 1] >> 7) & 0x01));
            }

            var inbit = GetBit(block[(segmentNumber) / 8], segmentNumber);
            iv[iv.Length - 1] =
                (byte)(((iv[iv.Length - 1] << 1) & 0xfe) | inbit);
        }

        private byte GetBit(byte ch, int i)
        {
            byte mask = 0x01;

            mask <<= 7 - (i % 8);

            if ((byte)(ch & mask) == mask)
            {
                return 1;
            }

            return 0;
        }

        void PutBit(ref byte outByte, byte data, int i)
        {
            byte mask = 0x01;

            mask <<= 7 - (i % 8);

            if ((byte)(data & 0x80) == 0x80)
            {
                outByte |= mask;
            }
            else
            {
                outByte &= (byte)~mask;
            }
        }
    }
}
