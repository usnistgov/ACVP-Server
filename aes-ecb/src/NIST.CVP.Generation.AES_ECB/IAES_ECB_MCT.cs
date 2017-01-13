using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface IAES_ECB_MCT
    {
        MCTResult MCTEncrypt(BitString key, BitString plainText);
        MCTResult MCTDecrypt(BitString key, BitString cipherText);
    }
}
