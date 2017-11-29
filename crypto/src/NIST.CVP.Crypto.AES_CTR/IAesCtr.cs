using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    public interface IAesCtr
    {
        EncryptionResult EncryptBlock(BitString key, BitString plainText, BitString iv);
        DecryptionResult DecryptBlock(BitString key, BitString cipherText, BitString iv);
        CounterEncryptionResult Encrypt(BitString key, BitString plainText, ICounter counter);
        CounterDecryptionResult Decrypt(BitString key, BitString cipherText, ICounter counter);
    }
}
