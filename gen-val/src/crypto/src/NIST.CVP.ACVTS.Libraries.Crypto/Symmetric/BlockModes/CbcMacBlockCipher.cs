﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes
{
    public class CbcMacBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        public override bool IsPartialBlockAllowed => false;

        public CbcMacBlockCipher(IBlockCipherEngine engine) : base(engine) { }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(_engine.BlockSizeBits);

            // Same logic regardless of encrypt/decrypt
            ProcessPayload(param, numberOfBlocks, outBuffer);

            return new SymmetricCipherResult(
                new BitString(outBuffer)
            );
        }

        private void ProcessPayload(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();

            // CBC-MAC by definition always uses an empty IV, so ignore the one provided
            var iv = new byte[_engine.BlockSizeBits];

            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                // XOR current block payload onto IV
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    iv[j] ^= payLoad[i * _engine.BlockSizeBytes + j];
                }

                _engine.ProcessSingleBlock(iv, outBuffer, 0);

                // Update Iv with current block's outBuffer values
                Array.Copy(outBuffer, 0, iv, 0, _engine.BlockSizeBytes);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}
