using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class DsaEccFactory : IDsaEccFactory
    {
        public IDsaEcc GetInstance(ISha sha)
        {
            return new EccDsa(sha);
        }
    }
}
