using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public interface ITDES_ECB
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText);
    }
}