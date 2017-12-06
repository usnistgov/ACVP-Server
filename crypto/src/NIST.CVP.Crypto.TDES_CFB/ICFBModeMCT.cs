using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public interface ICFBModeMCT
    {
        ICFBMode ModeOfOperation { get; set; }

        Algo Algo { get; set; }

        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString iv, BitString data);
    }
}