using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITDES_CBC_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv);
    }
}