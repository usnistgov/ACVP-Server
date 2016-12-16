using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public interface IRijndaelFactory
    {
        Rijndael GetRijndael(ModeValues mode);
    }
}
