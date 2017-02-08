using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CBC
{
    public interface IAES_CBC_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
