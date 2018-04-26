using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITDES_OFB
    {
        SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, BitString iv);
        SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString data, BitString iv);

    }
}
