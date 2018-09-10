using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Orleans.Grains.Hash
{
    public class OracleObserverShaMctCaseGrain : ObservableOracleGrainBase<MctResult<HashResult>>, 
        IOracleObserverShaMctCaseGrain
    {
        private readonly ISHA_MCT _shaMct;
        private readonly IEntropyProvider _entropyProvider;

        private ShaParameters _param;

        public OracleObserverShaMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISHA_MCT shaMct,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaMct = shaMct;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShaParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _entropyProvider.GetEntropy(_param.MessageLength);

            // TODO isSample up in here?
            var result = _shaMct.MCTHash(_param.HashFunction, message, false);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new MctResult<HashResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new HashResult {Message = element.Message, Digest = element.Digest})
            });
        }
    }
}
