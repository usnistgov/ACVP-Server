using System;

namespace NIST.CVP.Crypto.Core
{
    public class AesCipher : BlockCipher
    {
        public AesCipher()
        {
            throw new NotImplementedException();
        }

        public override int BlockSize => 128;
    }
}