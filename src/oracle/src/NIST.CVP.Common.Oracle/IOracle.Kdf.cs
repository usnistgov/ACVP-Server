using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KdfResult GetKdfCase();
        // All the components individually probably, but those are straight-forward input/output
    }
}
