using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Core
{
    public abstract class ConfidentialityMode : IModeOfOperation
    {
        protected ConfidentialityMode(BlockCipher cipher)
        {
            Cipher = cipher;
        }

        public BlockCipher Cipher { get; protected set; }
        public abstract EncryptionResult BlockEncrypt(BitString key,BitString iv, BitString plainText);
        public abstract DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}
