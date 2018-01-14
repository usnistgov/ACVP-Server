using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public static class ModeFactoryMCT
    {
        public static ICFBModeMCT GetMode(Algo algo)
        {
            var mode = ModeFactory.GetMode(algo);

            ICFBModeMCT modeMCT;
            switch (algo)
            {
                case Algo.TDES_CFB1:
                    modeMCT = new CFBModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case Algo.TDES_CFB8:
                    modeMCT = new CFBModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                case Algo.TDES_CFB64:
                    modeMCT = new CFBModeMCT(new MonteCarloKeyMaker(), mode, algo);
                    break;

                default:
                    modeMCT = null;
                    break;
            }
            return modeMCT;
        }
    }
}
