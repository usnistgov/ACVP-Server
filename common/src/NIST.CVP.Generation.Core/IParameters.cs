using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface IParameters
    {
        string Algorithm { get; }
        string Mode { get; }
        bool IsSample { get; }
    }
}
