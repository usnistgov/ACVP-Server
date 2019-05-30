using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class CbcCtsBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        private readonly ICiphertextStealingTransform _ciphertextStealingTransform;

        public CbcCtsBlockCipher(IBlockCipherEngine engine, ICiphertextStealingTransform ciphertextStealingTransform) :
            base(engine)
        {
            _ciphertextStealingTransform = ciphertextStealingTransform;
        }

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

            // Pad the last partial plaintext block with 0 until it reaches the block size.
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

            _ciphertextStealingTransform.TransformCiphertext(outBuffer, _engine, numberOfBlocks, param.Payload.BitLength);
        }

        private void Decrypt(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var payload = param.Payload.GetDeepCopy();
            var iv = param.Iv.ToBytes();

            // CS1 needs to decrypt the final full block then inject the last padded bits of the plaintext into the ciphertext starting at the end of the penultimate block
            // CS2/3 need to decrypt the second to the last block (which happens to be the last full block) and append that amount to pad onto the payload

            var transformedPayloadBytes =
                _ciphertextStealingTransform.HandleFinalFullPayloadBlockDecryption(payload, _engine, numberOfBlocks,
                    param.Payload.BitLength);

            // Decrypt the (modified) payload using the standard CBC mode.
            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                _engine.ProcessSingleBlock(transformedPayloadBytes, outBuffer, i);

                // XOR IV onto current block outBuffer
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] ^= iv[j];
                }

                // Update Iv with current block's payload values
                Array.Copy(transformedPayloadBytes, i * _engine.BlockSizeBytes, iv, 0, _engine.BlockSizeBytes);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}
