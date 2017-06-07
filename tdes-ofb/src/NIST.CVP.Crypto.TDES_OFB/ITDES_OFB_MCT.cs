using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFB
{
    public interface ITDES_OFB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv);
    }
}