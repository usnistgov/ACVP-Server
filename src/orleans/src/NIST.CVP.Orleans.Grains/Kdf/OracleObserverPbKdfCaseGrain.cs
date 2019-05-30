using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverPbKdfCaseGrain : ObservableOracleGrainBase<PbKdfResult>, 
        IOracleObserverPbKdfCaseGrain
    {
        private readonly IRandom800_90 _rand;
        private readonly IPbKdfFactory _factory;
        private PbKdfParameters _param;
        
        public OracleObserverPbKdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IPbKdfFactory factory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rand = rand;
            _factory = factory;
        }

        public async Task<bool> BeginWorkAsync(PbKdfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var pbkdf = _factory.GetKdf(_param.HashAlg);

            var salt = _rand.GetRandomBitString(_param.SaltLen);
            var password = _rand.GetRandomAlphaCharacters(_param.PassLen);

            var result = pbkdf.DeriveKey(salt, password, _param.ItrCount, _param.KeyLen);
            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new PbKdfResult
            {
                Password = password,
                DerivedKey = result.DerivedKey,
                Salt = salt
            });
        }
    }
}