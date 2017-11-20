using NIST.CVP.Crypto.TDES_CFBP;
using System;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public static class ModeFactory
    {
        public static IModeOfOperation GetMode(Algo algo)
        {
            IModeOfOperation mode;
            switch (algo)
            {
                case Algo.TDES_CFB1:
                case Algo.TDES_CFB8:
                case Algo.TDES_CFB64:
                    mode = new CFBMode(algo);
                    break;

                case Algo.TDES_CFBP1:
                case Algo.TDES_CFBP8:
                case Algo.TDES_CFBP64:
                    mode = new CFBPMode(algo);
                    break;

                case Algo.AES_CFB1:
                case Algo.AES_CFB8:
                case Algo.AES_CFB128:
                    throw  new NotImplementedException();

                default:
                    mode = null;
                    break;
            }
            return mode;
        }
    }
}
