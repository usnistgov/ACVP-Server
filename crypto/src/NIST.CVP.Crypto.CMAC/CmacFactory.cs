using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.CMAC.Enums;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacFactory : ICmacFactory
    {
        public ICmac GetCmacInstance(CmacTypes cmacType)
        {
            switch (cmacType)
            {
                case CmacTypes.AES128:
                case CmacTypes.AES192:
                case CmacTypes.AES256:
                    return new CmacAes(new RijndaelFactory(new RijndaelInternals()));
                    break;

                case CmacTypes.TDES:
                    return new CmacTdes(new TDES_ECB.TdesEcb());
                    break;
            }
            
            throw new ArgumentException($"Invalid {cmacType}");
        }
    }
}