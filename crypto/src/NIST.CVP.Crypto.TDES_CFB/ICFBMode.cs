using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public interface ICFBMode
    {
        Algo Algo { get; set; }
        DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);
        EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText);
    }
}
