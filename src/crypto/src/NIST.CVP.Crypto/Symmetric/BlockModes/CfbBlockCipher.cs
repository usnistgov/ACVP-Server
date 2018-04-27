using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class CfbBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        private readonly int _shift;

        public override bool IsPartialBlockAllowed => true;

        public CfbBlockCipher(IBlockCipherEngine engine, int shift) : base(engine)
        {
            // Valid shifts are 1 bit, 8 bits (byte), or the block size bits
            var validShifts = new[] { 1, 8, engine.BlockSizeBytes * BitsInByte };
            if (validShifts.Contains(shift))
            {
                _shift = shift;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(shift), 
                    "Invalid shift size. Must be 1 bit, 1 byte, or the block size."
                );
            }
        }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            var key = param.Key.ToBytes();

            // CFB always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfSegments = param.Payload.BitLength / _shift;
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            if (param.Direction == BlockCipherDirections.Encrypt)
            {
                Encrypt(param, numberOfSegments, outBuffer);
            }
            else
            {
                Decrypt(param, numberOfSegments, outBuffer);
            }

            return new SymmetricCipherResult(
                new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength)
            );
        }

        private void Encrypt(IModeBlockCipherParameters param, int numberOfSegments, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();
            var iv = param.Iv.GetDeepCopy().ToBytes();
            var ivOutBuffer = new byte[iv.Length];

            // For each segment
            for (int i = 0; i < numberOfSegments; i++)
            {
                _engine.ProcessSingleBlock(iv, ivOutBuffer, 0);

                // XOR processed IV onto current block payload
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] =
                        (byte)(payLoad[i * _engine.BlockSizeBytes + j] ^ ivOutBuffer[j]);
                }

                var outBufferStartIndex = i * (_engine.BlockSizeBytes / BitsInByte);

                // update next block IV to the processed outBuffer of this block
                Array.Copy(outBuffer, outBufferStartIndex, iv, 0, iv.Length);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }

        private void Decrypt(IModeBlockCipherParameters param, int numberOfSegments, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();
            var iv = param.Iv.GetDeepCopy().ToBytes();
            var ivOutBuffer = new byte[iv.Length];

            // For each segment
            for (int i = 0; i < numberOfSegments; i++)
            {
                _engine.ProcessSingleBlock(iv, ivOutBuffer, 0);

                // XOR processed IV onto current block payload
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] =
                        (byte)(payLoad[i * _engine.BlockSizeBytes + j] ^ ivOutBuffer[j]);
                }

                var payloadStartIndex = i * (_engine.BlockSizeBytes / BitsInByte);

                // update next block IV to the payload (ciphertext) of this block
                Array.Copy(payLoad, payloadStartIndex, iv, 0, iv.Length);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}