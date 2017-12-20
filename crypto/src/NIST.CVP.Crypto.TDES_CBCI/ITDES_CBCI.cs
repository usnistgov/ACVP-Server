using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CBCI
{
    public interface ITDES_CBCI
    {
        EncryptionResultWithIv BlockEncrypt(BitString key, BitString iv, BitString plainText);
        DecryptionResultWithIv BlockDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}