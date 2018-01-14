using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public static class ModeFactoryMCT
    {
        public static ICFBPModeMCT GetMode(Algo algo)
        {
            var mode = ModeFactory.GetMode(algo);

            ICFBPModeMCT modeMCT;
            switch (algo)
            {
                case Algo.TDES_CFBP1:
                    modeMCT = new CFBPModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case Algo.TDES_CFBP8:
                    modeMCT = new CFBPModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case Algo.TDES_CFBP64:
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
