using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public abstract class ConfidentialityModeMCT : IModeOfOperationMCT
    {
        protected IModeOfOperation ModeOfOperation { get; set; }
        protected ConfidentialityModeMCT(IModeOfOperation modeOfOperation)
        {
            ModeOfOperation = modeOfOperation;
        }
        

        public Algo Algo => ModeOfOperation.Algo;

        public abstract MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString iv, BitString data);
        public abstract MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString iv, BitString data);


    }
}
