using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_ECB
{
    public interface IAES_ECB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString cipherText);
    }
}
