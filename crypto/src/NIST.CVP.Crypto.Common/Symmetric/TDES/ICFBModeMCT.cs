using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ICFBModeMCT
    {
        ICFBMode ModeOfOperation { get; set; }

        Algo Algo { get; set; }

        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString iv, BitString data);
    }
}