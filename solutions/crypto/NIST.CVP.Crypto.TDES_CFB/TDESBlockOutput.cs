using NIST.CVP.Crypto.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class TDESBlockOutput : IBlockOutput
    {   
        public BitString Data { get; set; }
    }
}