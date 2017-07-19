using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CFB1
{
    public interface IAES_CFB1_MCT
    {
        MCTResult<BitOrientedAlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult<BitOrientedAlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
