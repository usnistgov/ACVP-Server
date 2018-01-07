using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFBI
{
    public interface ITDES_OFBI_MCT
    {
        MCTResult<AlgoArrayResponseWithIvs> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        MCTResult<AlgoArrayResponseWithIvs> MCTDecrypt(BitString keyBits, BitString iv, BitString data);
    }
}