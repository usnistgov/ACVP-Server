using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public interface IAesXts
    {
        EncryptionResult Encrypt(XtsKey key, BitString plainText, BitString i);
        DecryptionResult Decrypt(XtsKey key, BitString cipherText, BitString i);

        BitString GetIFromInteger(int dataUnitSeqNumber);
    }
}
