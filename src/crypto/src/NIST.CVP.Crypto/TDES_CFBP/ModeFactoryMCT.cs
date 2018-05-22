using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public static class ModeFactoryMCT
    {
        public static ICFBPModeMCT GetMode(AlgoMode algo)
        {
            var mode = ModeFactory.GetMode(algo);

            ICFBPModeMCT modeMCT;
            switch (algo)
            {
                case AlgoMode.TDES_CFBP1:
                    modeMCT = new CFBPModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case AlgoMode.TDES_CFBP8:
                    modeMCT = new CFBPModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case AlgoMode.TDES_CFBP64:
                    modeMCT = new CFBPModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                default:
                    modeMCT = null;
                    break;
            }
            return modeMCT;
        }
    }
}
