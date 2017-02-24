using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public interface ITDES_ECB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data);
    }
}