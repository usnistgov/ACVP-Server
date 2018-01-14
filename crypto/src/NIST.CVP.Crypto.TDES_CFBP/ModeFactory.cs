using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public static class ModeFactory
    {
        public static ICFBPMode GetMode(Algo algo)
        {
            ICFBPMode mode;
            switch (algo)
            {
                case Algo.TDES_CFBP1:
                case Algo.TDES_CFBP8:
                case Algo.TDES_CFBP64:
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
