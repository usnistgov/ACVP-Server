using NIST.CVP.Crypto.Common;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public static class ModeFactory
    {
        public static CFBMode GetMode(Algo algo)
        {
            CFBMode mode;
            switch (algo)
            {
                case Algo.TDES_CFB1:
                case Algo.TDES_CFB8:
                case Algo.TDES_CFB64:
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
