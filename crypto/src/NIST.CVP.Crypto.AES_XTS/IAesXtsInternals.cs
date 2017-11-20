using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_XTS
{
    public interface IAesXtsInternals
    {
        BitString MultiplyByAlpha(BitString encrypted_i, int j);
        BitString EncryptEcb(BitString key, BitString plainText);
        BitString DecryptEcb(BitString key, BitString cipherText);
    }
}
