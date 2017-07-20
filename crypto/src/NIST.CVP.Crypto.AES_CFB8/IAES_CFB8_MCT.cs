using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CFB8
{
    public interface IAES_CFB8_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
