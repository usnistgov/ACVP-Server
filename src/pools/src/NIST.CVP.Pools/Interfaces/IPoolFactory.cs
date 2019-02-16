using NIST.CVP.Common.Oracle;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IPoolFactory
    {
        IPool GetPool(PoolProperties poolProperties);
    }
}