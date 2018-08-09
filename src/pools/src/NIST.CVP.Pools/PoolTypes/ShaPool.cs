using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools.PoolTypes
{
    public class ShaPool : Pool<ShaParameters, HashResult>
    {
        public ShaPool(ShaParameters waterType, string filename) : base(waterType, filename) { }
    }
}
