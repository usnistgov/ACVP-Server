using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.Core
{
    public class ModeFactory
    {
        public IModeOfOperation GetMode(Algo algo)
        {
            IModeOfOperation mode;
            switch (algo)
            {
                case Algo.TDES_CFB1:
                    mode = new OFBMode(new TdesCipher(), 1);
                    break;

                case Algo.TDES_CFB8:
                    mode = new OFBMode(new TdesCipher(), 8);
                    break;

                case Algo.TDES_CFB64:
                    mode = new OFBMode(new TdesCipher(), 64);
                    break;

                case Algo.AES_CFB1:
                    mode = new OFBMode(new AesCipher(), 1);
                    break;

                case Algo.AES_CFB8:
                    mode = new OFBMode(new AesCipher(), 8);
                    break;

                case Algo.AES_CFB128:
                    mode = new OFBMode(new AesCipher(), 64);
                    break;

                default:
                    mode = null;
                        break;
            }
            return mode;
        }
    }
}
