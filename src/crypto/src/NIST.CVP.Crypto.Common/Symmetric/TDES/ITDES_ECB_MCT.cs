using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITDES_ECB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data);
    }
}