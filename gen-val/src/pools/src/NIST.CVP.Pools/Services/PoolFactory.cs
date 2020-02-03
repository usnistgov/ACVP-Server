using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;

namespace NIST.CVP.Pools.Services
{
    public class PoolFactory : IPoolFactory
    {
        private readonly IOptions<PoolConfig> _poolConfig;
        private readonly IOracle _oracle;
        private readonly IPoolRepositoryFactory _poolRepositoryFactory;
        private readonly IPoolLogRepository _poolLogRepository;
        private readonly IPoolObjectFactory _poolObjectFactory;
        private readonly Dictionary<string, long> _initialPoolCounts;

        public PoolFactory(
            IOptions<PoolConfig> poolConfig,
            IOracle oracle,
            IPoolRepositoryFactory poolRepositoryFactory,
            IPoolLogRepository poolLogRepository,
            IPoolObjectFactory poolObjectFactory,
            IPoolRepository<IResult> poolRepository
        )
        {
            _poolConfig = poolConfig;
            _oracle = oracle;
            _poolRepositoryFactory = poolRepositoryFactory;
            _poolLogRepository = poolLogRepository;
            _poolObjectFactory = poolObjectFactory;
            
            _initialPoolCounts = poolRepository.GetAllPoolCounts();
        }

        public IPool GetPool(PoolProperties poolProperties)
        {
            var param = poolProperties.PoolType.Parameters;

            switch (poolProperties.PoolType.Type)
            {
                // Primarily for testing purposes
                case PoolTypes.SHA:
                    return new ShaPool(GetConstructionParameters(param as ShaParameters, poolProperties));

                // Primarily for testing purposes
                case PoolTypes.AES:
                    return new AesPool(GetConstructionParameters(param as AesParameters, poolProperties));

                case PoolTypes.SHA_MCT:
                    return new ShaMctPool(GetConstructionParameters(param as ShaParameters, poolProperties));

                case PoolTypes.AES_MCT:
                    return new AesMctPool(GetConstructionParameters(param as AesParameters, poolProperties));

                case PoolTypes.TDES_MCT:
                    return new TdesMctPool(GetConstructionParameters(param as TdesParameters, poolProperties));

                case PoolTypes.SHA3_MCT:
                    return new Sha3MctPool(GetConstructionParameters(param as Sha3Parameters, poolProperties));

                case PoolTypes.CSHAKE_MCT:
                    return new CShakeMctPool(GetConstructionParameters(param as CShakeParameters, poolProperties));

                case PoolTypes.PARALLEL_HASH_MCT:
                    return new ParallelHashMctPool(GetConstructionParameters(param as ParallelHashParameters, poolProperties));

                case PoolTypes.TUPLE_HASH_MCT:
                    return new TupleHashMctPool(GetConstructionParameters(param as TupleHashParameters, poolProperties));

                case PoolTypes.DSA_PQG:
                    return new DsaPqgPool(GetConstructionParameters(param as DsaDomainParametersParameters, poolProperties));

                case PoolTypes.ECDSA_KEY:
                    return new EcdsaKeyPool(GetConstructionParameters(param as EcdsaKeyParameters, poolProperties));

                case PoolTypes.RSA_KEY:
                    return new RsaKeyPool(GetConstructionParameters(param as RsaKeyParameters, poolProperties));

                default:
                    throw new Exception("No pool model found");
            }
        }

        private PoolConstructionParameters<TParam> GetConstructionParameters<TParam>(TParam param, PoolProperties poolProperties)
            where TParam : IParameters
        {
            return new PoolConstructionParameters<TParam>()
            {
                Oracle = _oracle,
                PoolRepositoryFactory = _poolRepositoryFactory,
                PoolLogRepository = _poolLogRepository,
                PoolObjectFactory = _poolObjectFactory,
                PoolConfig = _poolConfig,
                PoolProperties = poolProperties,
                WaterType = param,
                PoolName = poolProperties.PoolName,
                PoolCount = _initialPoolCounts
                    .FirstOrDefault(f => f.Key.Equals(poolProperties.PoolName, StringComparison.OrdinalIgnoreCase)).Value
            };
        }
    }
}