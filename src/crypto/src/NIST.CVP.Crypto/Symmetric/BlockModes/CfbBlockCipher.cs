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
        private readonly IShiftRegisterStrategy _shiftRegisterStrategy;

        public override bool IsPartialBlockAllowed => true;

        public CfbBlockCipher(IBlockCipherEngine engine, IShiftRegisterStrategy shiftRegisterStrategy) : base(engine)
        {
            _shiftRegisterStrategy = shiftRegisterStrategy;
        }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            var key = param.Key.ToBytes();

            // CFB always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfSegments = param.Payload.BitLength / _shiftRegisterStrategy.ShiftSize;
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

                // XORs the payload onto the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetSegmentInProcessedBlock(payLoad, i, ivOutBuffer);

                // Sets the outbuffer segment equal to the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetOutBufferSegmentFromIvXorPayload(ivOutBuffer, i, outBuffer);

                // Sets up the iv for the next segment
                _shiftRegisterStrategy.SetNextRoundIv(ivOutBuffer, 0, iv);
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

                // XORs the payload onto the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetSegmentInProcessedBlock(payLoad, i, ivOutBuffer);

                // Sets the outbuffer segment equal to the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetOutBufferSegmentFromIvXorPayload(ivOutBuffer, i, outBuffer);

                // Sets up the iv for the next segment
                _shiftRegisterStrategy.SetNextRoundIv(payLoad, i, iv);
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}