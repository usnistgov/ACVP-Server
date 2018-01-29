using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class KeyPair
    {
        public PublicKey PubKey { get; set; }
        public PrivateKeyBase PrivKey { get; set; }
    }
}
