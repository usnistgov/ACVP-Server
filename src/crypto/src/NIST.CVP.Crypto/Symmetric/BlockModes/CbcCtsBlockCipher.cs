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
    public class CbcCtsBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        public CbcCtsBlockCipher(IBlockCipherEngine engine) : base(engine) { }

        public override bool IsPartialBlockAllowed => true;

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
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
            var iv = param.Iv.GetDeepCopy().ToBytes();

            // Pad the last partial plaintext block with 0.
            var paddedPayload = BitString.PadToModulus(param.Payload, _engine.BlockSizeBits).ToBytes();


            // Encrypt the whole padded plaintext using the standard CBC mode.
            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                // XOR IV onto current block payload
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    paddedPayload[i * _engine.BlockSizeBytes + j] ^= iv[j];
                }

                _engine.ProcessSingleBlock(paddedPayload, outBuffer, i);

                // Update Iv with current block's outBuffer values
                Array.Copy(outBuffer, i * _engine.BlockSizeBytes, iv, 0, _engine.BlockSizeBytes);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }

            // Swap the last two ciphertext blocks.
            if (numberOfBlocks > 1)
            {
                for (int i = 0; i < _engine.BlockSizeBytes; i++)
                {
                    var secondToLastBlockIndex = (numberOfBlocks - 2) * _engine.BlockSizeBytes + i;
                    var lastBlockIndex = (numberOfBlocks - 1) * _engine.BlockSizeBytes + i;

                    SwapBytes(outBuffer, secondToLastBlockIndex, lastBlockIndex);
                }
            }
        }

        private void Decrypt(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var payload = param.Payload.GetDeepCopy();
            var payloadBytes = payload.ToBytes();
            var iv = param.Iv.ToBytes();

            // Decrypt the second to last block using ecb mode
            if (numberOfBlocks > 1)
            {
                var originalPayload = param.Payload.ToBytes();

                var secondToLastBlock = new byte[_engine.BlockSizeBytes];
                var secondToLastBlockStartIndex = (numberOfBlocks - 2) * _engine.BlockSizeBytes;
                Array.Copy(originalPayload, secondToLastBlockStartIndex, secondToLastBlock, 0, _engine.BlockSizeBytes);

                var decryptedSecondToLastBlockBuffer = new byte[_engine.BlockSizeBytes];

                _engine.ProcessSingleBlock(secondToLastBlock, decryptedSecondToLastBlockBuffer, 0);

                var decryptedBlock = new BitString(decryptedSecondToLastBlockBuffer);

                // Pad the ciphertext to the nearest multiple of the block size using the last B−M bits of block cipher decryption of the second-to-last ciphertext block.
                var amountToPad = (_engine.BlockSizeBits - param.Payload.BitLength % _engine.BlockSizeBits);
                if (amountToPad > 0)
                {
                    payload = payload.ConcatenateBits(BitString.Substring(decryptedBlock, 0, amountToPad));
                }

                // Swap the last two ciphertext blocks.
                payloadBytes = payload.ToBytes();
                for (int i = 0; i < _engine.BlockSizeBytes; i++)
                {
                    var secondToLastBlockIndex = (numberOfBlocks - 2) * _engine.BlockSizeBytes + i;
                    var lastBlockIndex = (numberOfBlocks - 1) * _engine.BlockSizeBytes + i;

                    SwapBytes(payloadBytes, secondToLastBlockIndex, lastBlockIndex);
                }

                payload = new BitString(payloadBytes);
                payloadBytes = payload.ToBytes();
            }

            // Decrypt the (modified) ciphertext using the standard CBC mode.
            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                _engine.ProcessSingleBlock(payloadBytes, outBuffer, i);

                // XOR IV onto current block outBuffer
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] ^= iv[j];
                }

                // Update Iv with current block's payload values
                Array.Copy(payloadBytes, i * _engine.BlockSizeBytes, iv, 0, _engine.BlockSizeBytes);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }

        /// <summary>
        /// Swap two bytes within a byte array
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="byteA"></param>
        /// <param name="byteB"></param>
        private void SwapBytes(byte[] payload, int byteA, int byteB)
        {
            // x = x ^ y
            // y = x ^ y
            // x = x ^ y

            payload[byteA] = (byte)(payload[byteA] ^ payload[byteB]);
            payload[byteB] = (byte)(payload[byteA] ^ payload[byteB]);
            payload[byteA] = (byte)(payload[byteA] ^ payload[byteB]);
        }
    }
}
