using NIST.CVP.Math;
using NIST.CVP.Generation.TDES;

namespace NIST.CVP.Generation.TDES_CBC
{
    public interface ITDES_CBC
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString data, BitString iv);
    }
}
