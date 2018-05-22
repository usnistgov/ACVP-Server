using System;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

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
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}