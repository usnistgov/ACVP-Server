using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.FailureHandlers
{
    public interface IFailureReason
    {
        string GetName();
    }
}
