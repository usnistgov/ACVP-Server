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
    public class CbciBlockCipher : ModeBlockCipherBase<SymmetricCipherWithIvResult>
    {
        private const int PARTITIONS = 3;

        public override bool IsPartialBlockAllowed => false;

        public CbciBlockCipher(IBlockCipherEngine engine) : base(engine)
        {
            if (engine.BlockSizeBits != 64)
            {
                throw new NotSupportedException("Mode valid only with TDES Engine");
            }
        }

        public override SymmetricCipherWithIvResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            if (param.Payload.BitLength / _engine.BlockSizeBits < PARTITIONS)
            {
                throw new ArgumentException($"CBCI mode needs at least {PARTITIONS} blocks of data");
            }

            var payloads = TriPartitionBitString(param.Payload);
            var ivs = SetupIvs(param.Iv);
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            if (param.Direction == BlockCipherDirections.Encrypt)
            {
                Encrypt(param, payloads, ivs, outBuffer);
            }
            else
            {
                Decrypt(param, payloads, ivs, outBuffer);
            }

            return new SymmetricCipherWithIvResult(
                new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength),
                ivs
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

        private BitString[] TriPartitionBitString(BitString bitString)
        {
            //string needs to be evently splittable into three parts, and be on the byte boundary. 3 * 8 = 24
            if (bitString.BitLength % 24 != 0)
            {
                throw new Exception($"Can't tripartition a bitstring of size {bitString.BitLength}");
            }

            var retVal = new BitString[PARTITIONS];
            for (var i = 0; i < PARTITIONS; i++)
            {
                retVal[i] = new BitString(0);
            }

            for (var i = 0; i < bitString.BitLength / _engine.BlockSizeBits; i++)
            {
                var ptIndex = i % PARTITIONS;
                retVal[ptIndex] = retVal[ptIndex].ConcatenateBits(bitString.MSBSubstring(i * _engine.BlockSizeBits, _engine.BlockSizeBits));
            }

            return retVal;
        }

        private void Encrypt(
            IModeBlockCipherParameters param, 
            BitString[] payloads, 
            BitString[] ivs, 
            byte[] outBuffer
        )
        {
            byte[] iv = null;

            // for each partition
            for (int i = 0; i < PARTITIONS; i++)
            {
                iv = ivs[i].ToBytes();
                var payload = payloads[i].ToBytes();
                var tempOutBuffer = new byte[payload.Length];
                var numberOfBlocks = GetNumberOfBlocks(payloads[i].BitLength);

                // For each block
                for (int j = 0; j < numberOfBlocks; j++)
                {
                    // XOR IV onto current block payload
                    for (int k = 0; k < _engine.BlockSizeBytes; k++)
                    {
                        payload[j * _engine.BlockSizeBytes + k] ^= iv[k];
                    }

                    _engine.ProcessSingleBlock(payload, tempOutBuffer, j);

                    var outBufferIndex = ((j * PARTITIONS) + i) * _engine.BlockSizeBytes;
                    Array.Copy(
                        tempOutBuffer, 
                        j * _engine.BlockSizeBytes, 
                        outBuffer, 
                        outBufferIndex, 
                        _engine.BlockSizeBytes
                    );

                    // Update Iv with current block's outBuffer values
                    Array.Copy(tempOutBuffer, j * _engine.BlockSizeBytes, iv, 0, _engine.BlockSizeBytes);
                }
            }

            // Update the Param.Iv for the next call
            for (int i = 0; i < _engine.BlockSizeBytes; i++)
            {
                param.Iv[i] = iv[i];
            }
        }

        private void Decrypt(
            IModeBlockCipherParameters param,
            BitString[] payloads,
            BitString[] ivs,
            byte[] outBuffer
        )
        {
            byte[] iv = null;

            // for each partition
            for (int i = 0; i < PARTITIONS; i++)
            {
                iv = ivs[i].ToBytes();
                var payload = payloads[i].ToBytes();
                var tempOutBuffer = new byte[payload.Length];
                var numberOfBlocks = GetNumberOfBlocks(payloads[i].BitLength);

                // For each block
                for (int j = 0; j < numberOfBlocks; j++)
                {
                    _engine.ProcessSingleBlock(payload, tempOutBuffer, j);

                    // XOR IV onto temp outbuffer current block payload
                    for (int k = 0; k < _engine.BlockSizeBytes; k++)
                    {
                        tempOutBuffer[j * _engine.BlockSizeBytes + k] ^= iv[k];
                    }

                    var outBufferIndex = ((j * PARTITIONS) + i) * _engine.BlockSizeBytes;
                    Array.Copy(tempOutBuffer, j * _engine.BlockSizeBytes, outBuffer, outBufferIndex, _engine.BlockSizeBytes);

                    // Update Iv with current block's payload values
                    Array.Copy(payload, j * _engine.BlockSizeBytes, iv, 0, _engine.BlockSizeBytes);
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
