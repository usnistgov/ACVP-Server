using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 1 - final blocks never swapped, number of bits "stolen" from second to last block are dropped from the ciphertext
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf
    /// </summary>
    public class CiphertextStealingMode1 : ICiphertextStealingTransform
    {
        public void TransformCiphertext(byte[] outBuffer, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            if (numberOfBlocks == 1 || originalPayloadBitLength % engine.BlockSizeBits == 0)
            {
                return;
            }

            var bitsToAddForPadding = numberOfBlocks * engine.BlockSizeBits - originalPayloadBitLength;
            var partialBlockBits = engine.BlockSizeBits - bitsToAddForPadding;

            var partialBlockBytes = partialBlockBits / 8;

            // remove the "padded amount" of bits from the second to last block, shifting the amount removed onto it
            Array.Copy(outBuffer, outBuffer.Length - (1 * engine.BlockSizeBytes),
                outBuffer, outBuffer.Length - (2 * engine.BlockSizeBytes) + partialBlockBytes,
                engine.BlockSizeBytes
            );
        }

        public byte[] HandleFinalFullPayloadBlockDecryption(BitString payload, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // When payload is not a multiple of the block size
            if (numberOfBlocks > 1 && payload.BitLength % engine.BlockSizeBits != 0)
            {
                // decrypt the final block first, using ECB mode
                var finalBlock = payload.GetLeastSignificantBits(engine.BlockSizeBits).ToBytes();
                var finalBlockBuffer = new byte[engine.BlockSizeBytes];

                engine.ProcessSingleBlock(finalBlock, finalBlockBuffer, 0);

                var decryptedBlock = new BitString(finalBlockBuffer);

                // Pad the ciphertext to the nearest multiple of the block size using the last B−M bits of the decrypted final block.
                // These bits should be inserted after the penultimate block (full payload length minus a single block size).
                var amountToPad = (engine.BlockSizeBits - payload.BitLength % engine.BlockSizeBits);
                if (amountToPad > 0)
                {
                    //payload = payload.ConcatenateBits(BitString.Substring(decryptedBlock, 0, amountToPad));
                    payload = payload.GetMostSignificantBits(payload.BitLength - engine.BlockSizeBits)
                        .ConcatenateBits(decryptedBlock.GetLeastSignificantBits(amountToPad))
                        .ConcatenateBits(new BitString(finalBlock));
                }

                var payloadBytes = payload.ToBytes();

                return payloadBytes;
            }

            return payload.ToBytes();
        }
    }
}