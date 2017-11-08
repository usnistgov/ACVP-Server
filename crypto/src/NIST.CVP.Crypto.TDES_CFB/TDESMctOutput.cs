using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class TDESMctOutput : IMctOutput
    {
        public MCTResult<AlgoArrayResponse> Data { get; set; }
    }
}