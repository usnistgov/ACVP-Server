using System;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Helpers
{
    /// <summary>
    /// Helpers used for Pipeline and Interleaved modes
    /// </summary>
    public static class TdesPartitionHelpers
    {
        private const int PARTITIONS = 3;
        private const int BLOCK_SIZE_BITS = 64;
        private const int BITS_IN_BYTE = 8;

        public static BitString[] SetupIvs(BitString iv)
        {
            return new[]
            {
                iv.GetDeepCopy(),
                iv.AddWithModulo(new BitString("5555555555555555"), BLOCK_SIZE_BITS),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), BLOCK_SIZE_BITS)
            };
        }

        public static BitString[] TriPartitionBitString(BitString bitString)
        {
            //string needs to be evenly splittable into three parts, and be on the byte boundary. 3 * 8 = 24
            if (bitString.BitLength % (PARTITIONS * BITS_IN_BYTE) != 0)
            {
                throw new Exception($"Can't tripartition a bitstring of size {bitString.BitLength}");
            }

            var retVal = new BitString[PARTITIONS];
            for (var i = 0; i < PARTITIONS; i++)
            {
                retVal[i] = new BitString(0);
            }

            for (var i = 0; i < bitString.BitLength / BLOCK_SIZE_BITS; i++)
            {
                var ptIndex = i % PARTITIONS;
                retVal[ptIndex] = retVal[ptIndex].ConcatenateBits(bitString.MSBSubstring(i * BLOCK_SIZE_BITS, BLOCK_SIZE_BITS));
            }

            return retVal;
        }
    }
}
