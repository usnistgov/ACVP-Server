using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public KdfResult GetKdfCase() => throw new NotImplementedException();
        // All the components individually probably, but those are straight-forward input/output
    }
}
