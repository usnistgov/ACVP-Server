using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Ffx
{
    public abstract class FfxBlockCipherBase : IFfxModeBlockCipher
    {
        protected const int BitsInByte = 8;

        protected readonly IBlockCipherEngine _engine;
        protected readonly IModeBlockCipherFactory _factory;
        protected readonly IAesFfInternals _ffInternals;

        protected abstract int NumberOfRounds { get; }

        public bool IsPartialBlockAllowed => true;

        protected FfxBlockCipherBase(IBlockCipherEngine engine, IModeBlockCipherFactory factory, IAesFfInternals ffInternals)
        {
            // FFX only valid for AES
            if (engine.BlockSizeBits != 128)
            {
                throw new ArgumentException("FFX Mode(s) only valid with AES Engine");
            }

            _engine = engine;
            _factory = factory;
            _ffInternals = ffInternals;
        }

        public SymmetricCipherResult ProcessPayload(IFfxModeBlockCipherParameters param)
        {
            var key = param.Key.ToBytes();

            var engineParam = new EngineInitParameters(param.Direction, key, param.UseInverseCipherMode);
            _engine.Init(engineParam);

            CheckPayloadRequirements(param.Payload);

            if (param.Direction == BlockCipherDirections.Encrypt)
            {
                return new SymmetricCipherResult(Encrypt(param));
            }

            return new SymmetricCipherResult(Decrypt(param));
        }

        /// <summary>
        /// Checks that when partial blocks are not allowed, that the payload meets the block size
        /// </summary>
        /// <param name="payLoad"></param>
        private void CheckPayloadRequirements(BitString payLoad)
        {
            if (!IsPartialBlockAllowed)
            {
                if (payLoad.BitLength % (_engine.BlockSizeBytes / BitsInByte) != 0)
                {
                    throw new ArgumentException($"{nameof(payLoad)} must equal the Engine's block size");
                }
            }
        }

        protected abstract BitString Encrypt(IFfxModeBlockCipherParameters param);

        protected abstract BitString Decrypt(IFfxModeBlockCipherParameters param);
    }
}
