using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CBC
{
    public interface ITDES_CBC_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv);
    }
}