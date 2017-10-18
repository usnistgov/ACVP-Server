using System;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Core
{
    public interface IModeOfOperation
    {
        EncryptionResult BlockEncrypt(BitString key, BitString plainText, BitString iv);
        DecryptionResult BlockDecrypt(BitString key, BitString cipherText, BitString iv);
        
    }
}
