using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_ECB
{
    public interface ITDES_ECB
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText);
    }
}