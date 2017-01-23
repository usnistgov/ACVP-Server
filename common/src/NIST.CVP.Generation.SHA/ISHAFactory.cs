using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;


namespace NIST.CVP.Generation.SHA
{
    public interface ISHAFactory
    {
        SHA GetSHA(ModeValues mode);
    }
}
