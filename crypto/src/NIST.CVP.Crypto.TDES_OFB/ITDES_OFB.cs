using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFB
{
    public interface ITDES_OFB
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString data, BitString iv);

    }
}
