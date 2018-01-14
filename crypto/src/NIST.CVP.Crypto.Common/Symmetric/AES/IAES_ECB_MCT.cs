using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_ECB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString cipherText);
    }
}
