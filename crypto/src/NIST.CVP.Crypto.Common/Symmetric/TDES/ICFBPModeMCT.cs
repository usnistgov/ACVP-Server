using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ICFBPModeMCT
    {
        ICFBPMode ModeOfOperation { get; set; }

        Algo Algo { get; set; }

        MCTResult<AlgoArrayResponseWithIvs> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        MCTResult<AlgoArrayResponseWithIvs> MCTDecrypt(BitString keyBits, BitString iv, BitString data);
    }
}