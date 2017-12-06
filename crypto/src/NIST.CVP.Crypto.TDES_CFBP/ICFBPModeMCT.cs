using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public interface ICFBPModeMCT
    {
        ICFBPMode ModeOfOperation { get; set; }

        Algo Algo { get; set; }

        MCTResult<AlgoArrayResponseCfbp> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        MCTResult<AlgoArrayResponseCfbp> MCTDecrypt(BitString keyBits, BitString iv, BitString data);
    }
}