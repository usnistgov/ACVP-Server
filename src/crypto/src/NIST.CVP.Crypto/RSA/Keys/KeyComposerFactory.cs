﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2.Keys
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