using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface IAES_GCM
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv, BitString aad, int tagLength);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag);
    }
}
