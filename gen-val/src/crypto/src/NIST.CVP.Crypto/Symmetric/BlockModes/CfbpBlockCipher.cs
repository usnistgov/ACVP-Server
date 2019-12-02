using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class CfbpBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        private const int PARTITIONS = 3;
        private readonly IShiftRegisterStrategy _shiftRegisterStrategy;

        public override bool IsPartialBlockAllowed => true;

        public CfbpBlockCipher(IBlockCipherEngine engine, IShiftRegisterStrategy shiftRegisterStrategy) : base(engine)
        {
            _shiftRegisterStrategy = shiftRegisterStrategy;

            if (engine.BlockSizeBits != 64)
            {
                throw new NotSupportedException("Mode valid only with TDES Engine");
            }
        }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            if (param.Payload.BitLength % _shiftRegisterStrategy.ShiftSize != 0)
            {
                throw new ArgumentException("Payload length isn't evenly divisble by the segment size");
            }

            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();
            var actualBitsToProcess = param.Payload.BitLength;
            param.Payload = BitString.PadToNextByteBoundry(param.Payload);

            var ivs = SetupIvs(param.Iv);

            // CFB always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfSegments = actualBitsToProcess / _shiftRegisterStrategy.ShiftSize;
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            if (param.Direction == BlockCipherDirections.Encrypt)
            {
                Encrypt(param, numberOfSegments, ivs, outBuffer);
            }
            else
            {
                Decrypt(param, numberOfSegments, ivs, outBuffer);
            }

            return new SymmetricCipherResult(
                new BitString(outBuffer).GetMostSignificantBits(actualBitsToProcess)
            );
        }

        private BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new[]
            {
                iv,
                iv.AddWithModulo(new BitString("5555555555555555"), 64),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64)
            };
        }

        private void Encrypt(IModeBlockCipherParameters param, int numberOfSegments, BitString[] ivs, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();
            var ivOutBuffer = new byte[_engine.BlockSizeBytes];
            var iv = ivs[0].ToBytes();

            // For each segment
            for (int i = 0; i < numberOfSegments; i++)
            {
                _engine.ProcessSingleBlock(iv, ivOutBuffer, 0);

                // XORs the current segment of payload onto the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetSegmentInProcessedBlock(payLoad, i, ivOutBuffer);

                // Sets the outbuffer segment equal to the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetOutBufferSegmentFromIvXorPayload(ivOutBuffer, i, outBuffer);

                if (i + 1 < PARTITIONS)
                {
                    iv = ivs[(i + 1) % PARTITIONS].ToBytes();
                }
                else
                {
                    // TODO this without bitstrings, or take advantage of ShiftStretegy
                    iv = new BitString(iv)
                        .MSBSubstring(_shiftRegisterStrategy.ShiftSize, 64 - _shiftRegisterStrategy.ShiftSize)
                        .ConcatenateBits(
                            new BitString(outBuffer)
                                .MSBSubstring((i + 1 - PARTITIONS) * _shiftRegisterStrategy.ShiftSize,
                                    _shiftRegisterStrategy.ShiftSize)
                        ).ToBytes();
                }
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }

        private void Decrypt(IModeBlockCipherParameters param, int numberOfSegments, BitString[] ivs, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();
            var ivOutBuffer = new byte[_engine.BlockSizeBytes];
            var iv = ivs[0].ToBytes();

            // For each segment
            for (int i = 0; i < numberOfSegments; i++)
            {
                _engine.ProcessSingleBlock(iv, ivOutBuffer, 0);

                // XORs the current segment of payload onto the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetSegmentInProcessedBlock(payLoad, i, ivOutBuffer);

                // Sets the outbuffer segment equal to the first segment of the ivOutBuffer
                _shiftRegisterStrategy.SetOutBufferSegmentFromIvXorPayload(ivOutBuffer, i, outBuffer);

                // Sets up the iv for the next segment
                if (i + 1 < PARTITIONS)
                {
                    iv = ivs[(i + 1) % PARTITIONS].ToBytes();
                }
                else
                {
                    // TODO this without bitstrings, or take advantage of ShiftStretegy
                    iv = new BitString(iv)
                        .MSBSubstring(_shiftRegisterStrategy.ShiftSize, 64 - _shiftRegisterStrategy.ShiftSize)
                        .ConcatenateBits(
                            new BitString(payLoad)
                                .MSBSubstring((i + 1 - PARTITIONS) * _shiftRegisterStrategy.ShiftSize,
                                    _shiftRegisterStrategy.ShiftSize)
                        ).ToBytes();
                }
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }
    }
}