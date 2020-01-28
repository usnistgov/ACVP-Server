using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.Engines;
using System;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Aead
{
    public class AeadModeBlockCipherFactory : IAeadModeBlockCipherFactory
    {
        public IAeadModeBlockCipher GetAeadCipher(
            IBlockCipherEngine engine, 
            BlockCipherModesOfOperation modeOfOperation
        )
        {
            switch (modeOfOperation)
            {
                case BlockCipherModesOfOperation.Ccm:
                    return new CcmBlockCipher(engine, new ModeBlockCipherFactory(), new AES_CCMInternals());
                case BlockCipherModesOfOperation.Gcm:
                    return new GcmBlockCipher(engine, new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));
                case BlockCipherModesOfOperation.GcmSiv:
                    return new GcmSivBlockCipher(engine, new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}