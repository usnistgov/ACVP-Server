using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public class KeyComposerFactory : IKeyComposerFactory
    {
        public IRsaKeyComposer GetKeyComposer(PrivateKeyModes privateKeyModes)
        {
            switch (privateKeyModes)
            {
                case PrivateKeyModes.Standard:
                    return new RsaKeyComposer();

                case PrivateKeyModes.Crt:
                    return new CrtKeyComposer();

                default:
                    throw new ArgumentException("Invalid private key mode");
            }
        }
    }
}
