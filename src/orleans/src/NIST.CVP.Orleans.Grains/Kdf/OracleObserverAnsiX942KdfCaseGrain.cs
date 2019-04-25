using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Orleans.Grains.Kdf
{
    public class OracleObserverAnsiX942KdfCaseGrain : ObservableOracleGrainBase<AnsiX942KdfResult>, IOracleObserverAnsiX942KdfCaseGrain
    {
        private readonly IAnsiX942Factory _factory;
        private readonly IRandom800_90 _rand;

        private AnsiX942Parameters _param;

        public OracleObserverAnsiX942KdfCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IAnsiX942Factory factory, IRandom800_90 rand) : base(nonOrleansScheduler)
        {
            _factory = factory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(AnsiX942Parameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var kdf = _factory.GetInstance(_param.KdfMode, _param.HashAlg);

            var zz = _rand.GetRandomBitString(_param.ZzLen);
            var otherInfo = _rand.GetRandomBitString(_param.OtherIntoLen);

            var result = kdf.DeriveKey(zz, _param.KeyLen, otherInfo);
            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            await Notify(new AnsiX942KdfResult
            {
                DerivedKey = result.DerivedKey,
                OtherInfo = otherInfo,
                Zz = zz
            });
        }
    }
}
