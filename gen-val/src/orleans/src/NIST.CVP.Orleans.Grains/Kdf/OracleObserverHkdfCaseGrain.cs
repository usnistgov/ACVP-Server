using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.HKDF;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverHkdfCaseGrain : ObservableOracleGrainBase<HkdfResult>, IOracleObserverHkdfCaseGrain
    {
        private readonly IRandom800_90 _rand;
        private readonly IHkdfFactory _factory;
        private HkdfParameters _param;
    
        public OracleObserverHkdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IHkdfFactory factory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rand = rand;
            _factory = factory;
        }

        public async Task<bool> BeginWorkAsync(HkdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
    
        protected override async Task DoWorkAsync()
        {
            var hkdf = _factory.GetKdf(_param.HmacAlg);

            var salt = _rand.GetRandomBitString(_param.SaltLen);
            var inputKeyingMaterial = _rand.GetRandomBitString(_param.InputLen);
            var otherInfo = _rand.GetRandomBitString(_param.InfoLen);

            var result = hkdf.DeriveKey(salt, inputKeyingMaterial, otherInfo, _param.KeyLen);
            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new HkdfResult
            {
                InputKeyingMaterial = inputKeyingMaterial,
                OtherInfo = otherInfo,
                DerivedKey = result.DerivedKey,
                Salt = salt
            });
        }
    }
}