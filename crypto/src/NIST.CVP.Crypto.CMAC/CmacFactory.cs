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
                case CmacTypes.AES:
                    return new CmacAes(new RijndaelFactory(new RijndaelInternals()));
            }
            
            throw new ArgumentException($"Invalid {cmacType}");
        }
    }
}