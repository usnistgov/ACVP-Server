using NIST.CVP.Crypto.CTR;
using NIST.CVP.Crypto.TDES;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CTR
{
    public interface ITdesCtr
    {
        EncryptionResult EncryptBlock(BitString key, BitString plainText, BitString iv);
        DecryptionResult DecryptBlock(BitString key, BitString cipherText, BitString iv);
        CounterEncryptionResult Encrypt(BitString key, BitString plainText, ICounter counter);
        CounterDecryptionResult Decrypt(BitString key, BitString cipherText, ICounter counter);
    }
}
