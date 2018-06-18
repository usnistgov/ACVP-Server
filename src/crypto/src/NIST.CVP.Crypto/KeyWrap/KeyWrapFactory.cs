using System;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapFactory : IKeyWrapFactory
    {
        public IKeyWrap GetKeyWrapInstance(KeyWrapType keyWrapType)
        {
            switch (keyWrapType)
            {
                case KeyWrapType.AES_KW:
                    return new KeyWrapAes(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
                case KeyWrapType.TDES_KW:
                    return new KeyWrapTdes(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
                case KeyWrapType.AES_KWP:
                    return new KeyWrapWithPaddingAes(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
                default:
                    throw new ArgumentException($"Invalid {nameof(KeyWrapType)} provided.");
            }
        }
    }
}
