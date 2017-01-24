using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public interface IAES_OFB
    {
        EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString plainText);
        DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
