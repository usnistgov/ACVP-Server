using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface IAES_ECB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString cipherText);
    }
}
