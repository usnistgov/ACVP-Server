using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public static class ModeFactoryMCT
    {
        public static IModeOfOperationMCT GetMode(Algo algo)
        {
            var mode = ModeFactory.GetMode(algo);

            IModeOfOperationMCT modeMCT;
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
