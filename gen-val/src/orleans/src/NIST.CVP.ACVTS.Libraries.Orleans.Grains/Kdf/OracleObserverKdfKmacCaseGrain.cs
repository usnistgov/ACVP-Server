using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverKdfKmacCaseGrain : ObservableOracleGrainBase<KdfKmacResult>, IOracleObserverKdfKmacCaseGrain
    {
        private readonly IRandom800_90 _rand;
        private readonly IKdfFactory _kdfFactory;
        private KdfKmacParameters _param;
        
        public OracleObserverKdfKmacCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKdfFactory factory,
            IRandom800_90 rand) : base(nonOrleansScheduler)
        {
            _rand = rand;
            _kdfFactory = factory;
        }

        public async Task<bool> BeginWorkAsync(KdfKmacParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            try
            {
                var kdf = _kdfFactory.GetKdfInstance(KdfModes.Kmac, _param.MacMode, CounterLocations.None);

                var keyDerivationKey = _rand.GetRandomBitString(_param.KeyDerivationKeyLength);
                var context = _rand.GetRandomBitString(_param.ContextLength);
                var label = _rand.GetRandomBitString(_param.LabelLength);

                var result = kdf.DeriveKey(keyDerivationKey, context, _param.DerivedKeyLength, label);
                if (!result.Success)
                {
                    await Throw(new Exception(result.ErrorMessage));
                    return;
                }

                await Notify(new KdfKmacResult
                {
                    Context = context,
                    DerivedKey = result.DerivedKey,
                    KeyDerivationKey = keyDerivationKey,
                    Label = label
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
