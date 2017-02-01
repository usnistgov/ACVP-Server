using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1
{
    public interface IAES_CFB1_MCT
    {
        MCTResult MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
