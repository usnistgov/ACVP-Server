using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class EcbBlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        public override bool IsPartialBlockAllowed => false;

        public EcbBlockCipher(IBlockCipherEngine engine) : base(engine) { }

        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            CheckPayloadRequirements(param.Payload);
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            var numberOfBlocks = GetNumberOfBlocks(param.Payload.BitLength);
            var outBuffer = GetOutputBuffer(param.Payload.BitLength);

            // block setup is same between encrypt/decrypt
            ProcessPayload(param, numberOfBlocks, outBuffer);

            return new SymmetricCipherResult(new BitString(outBuffer).GetMostSignificantBits(param.Payload.BitLength));
        }
        
        private void ProcessPayload(IModeBlockCipherParameters param, int numberOfBlocks, byte[] outBuffer)
        {
            var payLoad = param.Payload.ToBytes();

            for (int i = 0; i < numberOfBlocks; i++)
            {
                _engine.ProcessSingleBlock(payLoad, outBuffer, i);
            }
        }
    }
}