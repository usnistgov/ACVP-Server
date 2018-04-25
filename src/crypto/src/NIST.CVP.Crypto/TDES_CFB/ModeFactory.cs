using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public static class ModeFactory
    {
        public static CFBMode GetMode(AlgoMode algo)
        {
            CFBMode mode;
            switch (algo)
            {
                case AlgoMode.TDES_CFB1:
                case AlgoMode.TDES_CFB8:
                case AlgoMode.TDES_CFB64:
                    mode = new CFBMode(algo);
                    break;
                default:
                    mode = null;
                    break;
            }
            return mode;
        }
    }
}
