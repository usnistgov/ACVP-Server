using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverAnsiX963KdfCaseGrain : ObservableOracleGrainBase<AnsiX963KdfResult>,
        IOracleObserverAnsiX963KdfCaseGrain
    {
        private readonly IAnsiX963Factory _kdfFactory;
        private readonly IRandom800_90 _rand;

        private AnsiX963Parameters _param;

        public OracleObserverAnsiX963KdfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IAnsiX963Factory kdfFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _kdfFactory = kdfFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(AnsiX963Parameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var ansi = _kdfFactory.GetInstance(_param.HashAlg);

            var z = _rand.GetRandomBitString(_param.FieldSize).PadToModulus(8);
            var sharedInfo = _rand.GetRandomBitString(_param.SharedInfoLength);

            var result = ansi.DeriveKey(z, sharedInfo, _param.KeyDataLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new AnsiX963KdfResult
            {
                Z = z,
                SharedInfo = sharedInfo,
                KeyOut = result.DerivedKey
            });
        }
    }
}
