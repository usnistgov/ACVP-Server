using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Models
{
    public class PoolConstructionParameters<TParam>
        where TParam : IParameters
    {
        public IOracle Oracle { get; set; }
        public IPoolRepositoryFactory PoolRepositoryFactory { get; set; }
        public IPoolLogRepository PoolLogRepository { get; set; }
        public IPoolObjectFactory PoolObjectFactory { get; set; }
        public IOptions<PoolConfig> PoolConfig { get; set; }
        public TParam WaterType { get; set; }
        public PoolProperties PoolProperties { get; set; }
        public string PoolName { get; set; }
        public long PoolCount { get; set; }
    }
}
