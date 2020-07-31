using System.Net.Http;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
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
        private readonly IKeyComposerFactory _rsaKeyComposerFactory;
        private readonly IHttpClientFactory _httpClientFactory;

        public OraclePools(
            IClusterClientFactory clusterClientFactory,
            IOptions<OrleansConfig> orleansConfig,
            IOptions<PoolConfig> poolConfig,
            IHttpClientFactory httpClientFactory
        ) : base(clusterClientFactory, orleansConfig)
        {
            _rsaKeyComposerFactory = new KeyComposerFactory();
            _poolConfig = poolConfig;
            _httpClientFactory = httpClientFactory;
        }
        
        public override async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<AesResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.AES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetAesMctCaseAsync(param);
        }

        public override async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                // Will return a G (and some other properties) that are not necessary
                return poolResult;
            }

            return await base.GetDsaPQAsync(param);
        }

        public override async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetDsaDomainParametersAsync(param);
        }

        public override async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            var poolBoy = new PoolBoy<EcdsaKeyResult>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.ECDSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetEcdsaKeyAsync(param);
        }

        public override async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.SHA_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            return await base.GetShaMctCaseAsync(param);
        }

        public override async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.SHA3_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetSha3MctCaseAsync(param);
        }

        public override async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<CShakeResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.CSHAKE_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetCShakeMctCaseAsync(param);
        }

        public override async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<ParallelHashResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.PARALLEL_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetParallelHashMctCaseAsync(param);
        }

        public override async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TupleHashResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.TUPLE_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetTupleHashMctCaseAsync(param);
        }

        public override async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TdesResult>>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.TDES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            return await base.GetTdesMctCaseAsync(param);
        }

        public override async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            // Only works with random public exponent
            var poolBoy = new PoolBoy<RsaKeyResult>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.RSA_KEY);

            // No pool available, generate on-demand
            if (poolResult == null) return await base.GetRsaKeyAsync(param);
            
            // Pool available, format key properly. Pools ONLY generate "standard" keys.
            var keyComposer = _rsaKeyComposerFactory.GetKeyComposer(param.KeyFormat);
            poolResult.Key = keyComposer.ComposeKey(poolResult.Key.PubKey.E,
                new PrimePair {P = poolResult.Key.PrivKey.P, Q = poolResult.Key.PrivKey.Q});

            return poolResult;
        }

        public override async Task<DsaKeyResult> GetSafePrimeKeyAsync(SafePrimesKeyGenParameters param)
        {
            // Only works with random public exponent
            var poolBoy = new PoolBoy<DsaKeyResult>(_poolConfig, _httpClientFactory);
            var poolResult = await poolBoy.GetObjectFromPoolAsync(param, PoolTypes.SafePrime_Key);

            // No pool available, generate on-demand
            if (poolResult == null) 
                return await base.GetSafePrimeKeyAsync(param);
            
            return poolResult;
        }
    }
}
