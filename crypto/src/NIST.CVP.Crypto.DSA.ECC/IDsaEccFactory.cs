using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IDsaEccFactory
    {
        IDsaEcc GetInstance(ISha sha);
    }
}
