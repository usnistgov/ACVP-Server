using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CBC
{
    public interface ITDES_CBC
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString data, BitString iv);
    }
}
