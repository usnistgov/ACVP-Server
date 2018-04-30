using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class CtrBlockCipher : ModeBlockCipherBase<SymmetricCounterResult>
    {
        private readonly ICounter _counter;

        public CtrBlockCipher(IBlockCipherEngine engine, ICounter counter)
            : base(engine)
        {
            _counter = counter;
        }

        public override bool IsPartialBlockAllowed => true;

        public override SymmetricCounterResult ProcessPayload(IModeBlockCipherParameters param)
        {
            var ivs = new List<BitString>();
            var key = param.Key.ToBytes();
            var actualBitsToProcess = param.Payload.BitLength;
            param.Payload = BitString.PadToModulus(param.Payload, _engine.BlockSizeBits);

            // CTR always utilizes engine in encrypt mode
            var engineParam = new EngineInitParameters(BlockCipherDirections.Encrypt, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            ProcessPayload(param, numberOfBlocks, outBuffer, ivs);
            
            return new SymmetricCounterResult(
                new BitString(outBuffer).GetMostSignificantBits(actualBitsToProcess),
                ivs
            );
        }

        private void ProcessPayload(
            IModeBlockCipherParameters param, 
            int numberOfBlocks, 
            byte[] outBuffer, 
            List<BitString> ivs
        )
        {
            var payLoad = param.Payload.ToBytes();

            // For each block
            for (int i = 0; i < numberOfBlocks; i++)
            {
                var iv = _counter.GetNextIV();
                ivs.Add(iv);
                var ivBytes = iv.ToBytes();
                var ivOutBuffer = new byte[ivBytes.Length];

                _engine.ProcessSingleBlock(ivBytes, ivOutBuffer, 0);

                // XOR processed IV onto current block payload
                for (int j = 0; j < _engine.BlockSizeBytes; j++)
                {
                    outBuffer[i * _engine.BlockSizeBytes + j] =
                        (byte)(payLoad[i * _engine.BlockSizeBytes + j] ^ ivOutBuffer[j]);
                }
            }
        }
    }
}