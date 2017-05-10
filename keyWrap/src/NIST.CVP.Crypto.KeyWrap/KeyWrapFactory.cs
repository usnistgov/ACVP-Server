using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.KeyWrap.Enums;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapFactory : IKeyWrapFactory
    {
        public IKeyWrap GetKeyWrapInstance(KeyWrapType keyWrapType)
        {
            switch (keyWrapType)
            {
                case KeyWrapType.AES_KW:
                    return new KeyWrapAes(new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));
                default:
                    throw new ArgumentException($"Invalid {nameof(KeyWrapType)} provided.");
            }
        }
    }
}
