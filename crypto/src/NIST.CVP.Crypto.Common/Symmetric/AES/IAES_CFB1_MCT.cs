using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_CFB1_MCT
    {
        MCTResult<BitOrientedAlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult<BitOrientedAlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
