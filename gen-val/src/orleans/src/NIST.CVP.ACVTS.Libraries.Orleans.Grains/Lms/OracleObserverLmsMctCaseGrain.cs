using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms
{
    public class OracleObserverLmsMctCaseGrain : ObservableOracleGrainBase<MctResult<LmsSignatureResult>>,
        IOracleObserverLmsMctCaseGrain
    {
        private readonly ILmsMct _lmsMct;
        private readonly IRandom800_90 _rand;

        private LmsSignatureParameters _param;

        public OracleObserverLmsMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ILmsMct lmsMct,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _lmsMct = lmsMct;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(LmsSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(256);

            var seed = _rand.GetRandomBitString(256);

            var i = _rand.GetRandomBitString(128);

            var result = await _lmsMct.MCTHashAsync(_param.LmsTypes, _param.LmotsTypes, seed, i, message, _param.IsSample);

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new MctResult<LmsSignatureResult>
            {
                Seed = new LmsSignatureResult()
                {
                    Message = message,
                    SEED = seed,
                    I = i
                },
                Results = result.Response.ConvertAll(element =>
                    new LmsSignatureResult
                    {
                        Message = element.Message,
                        Signature = element.Signature
                    })
            });
        }
    }
}
