using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    /// <summary>
    /// Adapted from https://github.com/bcgit/bc-csharp
    /// </summary>
    public abstract class NativeFastSha2SmallBlockBase
    {
        private byte[] xBuf = new byte[4];
        private int xBufOff;
        private int xBitOff;
        private long bitCount;

        protected abstract void ProcessWord(byte[] input, int inOff);
        protected abstract void ProcessWord(uint input);
        protected abstract void ProcessLength(long bitLength);
        protected abstract void ProcessBlock();

        protected void Finish()
        {
            // Next bit is 1, remainder of xBuf is 0
            xBuf[xBufOff] |= (byte)(0x80 >> xBitOff);

            // Complete byte with 0
            xBuf[xBufOff++] &= (byte)(0xFF << (8 - xBitOff - 1));

            if (xBufOff == 4)
            {
                ProcessWord(xBuf, 0);
                xBufOff = 0;
            }

            while (xBufOff != 0)
            {
                xBuf[xBufOff++] = 0x00;
                if (xBufOff == 4)
                {
                    ProcessWord(xBuf, 0);
                    xBufOff = 0;
                }
            }

            ProcessLength(bitCount);
            ProcessBlock();
        }

        public void Update(byte[] input, int bitLength)
        {
            // If there is an in-process word
            var inputByteOff = 0;
            var inputBitOff = 0;
            var completeBytes = bitLength / 8;
            if (xBufOff != 0 || xBitOff != 0)
            {
                // For each byte in the input
                while (inputByteOff < completeBytes)
                {
                    // Complete current byte
                    xBuf[xBufOff++] |= (byte)(input[inputByteOff] >> xBitOff);
                    inputBitOff = (8 - xBitOff) % 8;
                    xBitOff = 0;

                    // Stop if word is completed
                    if (xBufOff == xBuf.Length)
                    {
                        if (inputBitOff == 0)
                        {
                            inputByteOff++;
                        }

                        ProcessWord(xBuf, 0);
                        xBufOff = 0;
                        Array.Clear(xBuf, 0, xBuf.Length);
                        break;
                    }

                    if (inputByteOff != input.Length && inputBitOff == 0)
                    {
                        inputByteOff++;
                        continue;
                    }

                    // Take remainder of input byte
                    xBuf[xBufOff] |= (byte)(input[inputByteOff++] << inputBitOff);
                    xBitOff = (8 - inputBitOff) % 8;
                }
            }

            // Process all completed words by grabbing segments of 32-bits until no more 32-bit segments remain
            uint nextWord = 0;
            int nextByte;
            while (inputByteOff * 8 + inputBitOff <= bitLength - 32)
            {
                // If there's no bits involved, don't need to do any shifts
                if (inputBitOff == 0)
                {
                    ProcessWord(input, inputByteOff);
                }
                else
                {
                    nextByte = (byte)(input[inputByteOff] << inputBitOff) | (byte)(input[inputByteOff + 1] >> (8 - inputBitOff));
                    nextWord = (uint)nextByte << 24;

                    nextByte = (byte)(input[inputByteOff + 1] << inputBitOff) | (byte)(input[inputByteOff + 2] >> (8 - inputBitOff));
                    nextWord |= (uint)nextByte << 16;

                    nextByte = (byte)(input[inputByteOff + 2] << inputBitOff) | (byte)(input[inputByteOff + 3] >> (8 - inputBitOff));
                    nextWord |= (uint)nextByte << 8;

                    nextByte = (byte)(input[inputByteOff + 3] << inputBitOff) | (byte)(input[inputByteOff + 4] >> (8 - inputBitOff));
                    nextWord |= (uint)nextByte;

                    ProcessWord(nextWord);
                }

                inputByteOff += 4;
            }

            // Load in remainder
            while (inputByteOff * 8 + inputBitOff < bitLength)
            {
                // Load in one bit at a time
                xBuf[xBufOff] |= (byte)((byte)(input[inputByteOff] << inputBitOff++) >> xBitOff++);

                if (xBitOff == 8)
                {
                    xBitOff = 0;
                    xBufOff++;
                }

                if (inputBitOff == 8)
                {
                    inputBitOff = 0;
                    inputByteOff++;
                }
            }

            bitCount += bitLength;
        }

        protected void Init()
        {
            bitCount = 0;
            xBufOff = 0;
            xBitOff = 0;
            Array.Clear(xBuf, 0, xBuf.Length);
        }
    }
}
