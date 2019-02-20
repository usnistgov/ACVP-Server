using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System.Threading.Tasks;
using NIST.CVP.Common.Interfaces;

namespace NIST.CVP.Crypto.Oracle
{
    public class OraclePools : Oracle
    {
        private readonly IOptions<PoolConfig> _poolConfig;

        public OraclePools(
            IDbConnectionStringFactory dbConnectionStringFactory,
            IOptions<EnvironmentConfig> environmentConfig, 
            IOptions<OrleansConfig> orleansConfig,
            IOptions<PoolConfig> poolConfig
        ) : base(dbConnectionStringFactory, environmentConfig, orleansConfig)
        {
            _poolConfig = poolConfig;
        }
        
        public override async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<AesResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.AES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetAesMctCaseAsync(param);
        }

        public override async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                // Will return a G (and some other properties) that are not necessary
                return poolResult;
            }

            return await base.GetDsaPQAsync(param);
        }

        public override async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetDsaDomainParametersAsync(param);
        }

        public override async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            var poolBoy = new PoolBoy<EcdsaKeyResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.ECDSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetEcdsaKeyAsync(param);
        }

        public override async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            return await base.GetShaMctCaseAsync(param);
        }

        public override async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA3_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetSha3MctCaseAsync(param);
        }

        public override async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<CShakeResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.CSHAKE_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetCShakeMctCaseAsync(param);
        }

        public override async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<ParallelHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.PARALLEL_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetParallelHashMctCaseAsync(param);
        }

        public override async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TupleHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TUPLE_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetTupleHashMctCaseAsync(param);
        }

        public override async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TdesResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TDES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetTdesMctCaseAsync(param);
        }

        public override async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            // Only works with random public exponent
            var poolBoy = new PoolBoy<RsaKeyResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.RSA_KEY);

            if (poolResult != null)
            {
                if (param.KeyFormat == PrivateKeyModes.Crt)
                {
                    var crtKeyComposer = new CrtKeyComposer();
                    poolResult.Key = crtKeyComposer.ComposeKey(
                        poolResult.Key.PubKey.E,
                        new PrimePair
                        {
                            P = poolResult.Key.PrivKey.P,
                            Q = poolResult.Key.PrivKey.Q
                        });
                }

                return poolResult;
            }

            return await base.GetRsaKeyAsync(param);
        }
    }
}
