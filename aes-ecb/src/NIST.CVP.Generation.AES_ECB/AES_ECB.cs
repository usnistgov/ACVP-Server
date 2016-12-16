using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class AES_ECB : IAES_ECB
    {
        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText)
        {
            throw new NotImplementedException();
        }

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data)
        {
            throw new NotImplementedException();
        }
    }
}
