using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Cshake;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Cshake
{
    public class OracleObserverCShakeMctCaseGrain : ObservableOracleGrainBase<MctResult<CShakeResult>>,
        IOracleObserverCShakeMctCaseGrain
    {
        private readonly IcSHAKE_MCT _cshakeMct;
        private readonly IRandom800_90 _rand;

        private CShakeParameters _param;

        public OracleObserverCShakeMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IcSHAKE_MCT cshakeMct,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _cshakeMct = cshakeMct;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(CShakeParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);

            var result = _cshakeMct.MCTHash(_param.HashFunction, message, _param.OutLens, _param.HexCustomization, _param.IsSample);

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new MctResult<CShakeResult>
            {
                Seed = new CShakeResult()
                {
                    Message = message,
                    Customization = "",        // These values start out empty
                    FunctionName = ""
                },
                Results = result.Response.ConvertAll(element =>
                    new CShakeResult
                    {
                        Message = element.Message,
                        Digest = element.Digest,
                        Customization = element.Customization,
                        FunctionName = element.FunctionName
                    })
            });
        }
    }
}
