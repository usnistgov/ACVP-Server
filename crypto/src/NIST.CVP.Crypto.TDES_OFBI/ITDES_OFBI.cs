using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFBI
{
    public interface ITDES_OFBI
    {
        EncryptionResultWithIv BlockEncrypt(BitString key, BitString iv, BitString plainText);
        DecryptionResultWithIv BlockDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}