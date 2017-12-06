using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public interface ICFBPMode
    {
        Algo Algo { get; set; }
        DecryptionResultWithIv BlockDecrypt(BitString key, BitString iv, BitString cipherText, bool includeAuxValues = false);
        DecryptionResultWithIv BlockDecrypt(BitString key, BitString iv, BitString cipherText1, BitString cipherText2, BitString cipherText3);
        EncryptionResultWithIv BlockEncrypt(BitString key, BitString iv, BitString plainText, bool includeAuxValues = false);
        EncryptionResultWithIv BlockEncrypt(BitString key, BitString iv, BitString plainText1, BitString plainText2, BitString plainText3);
    }
}
