using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public static class ModeFactory
    {
        public static ICFBPMode GetMode(AlgoMode algo)
        {
            ICFBPMode mode;
            switch (algo)
            {
                case AlgoMode.TDES_CFBP1:
                case AlgoMode.TDES_CFBP8:
                case AlgoMode.TDES_CFBP64:
                    mode = new CFBPMode(algo);
                    break;
                default:
                    mode = null;
                    break;
            }
            return mode;
        }
    }
}
