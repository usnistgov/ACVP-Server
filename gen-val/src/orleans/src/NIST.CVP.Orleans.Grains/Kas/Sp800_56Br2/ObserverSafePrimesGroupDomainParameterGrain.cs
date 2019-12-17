using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Br2
{
    public class ObserverSafePrimesGroupDomainParameterGrain : ObservableOracleGrainBase<FfcDomainParameters>, IObserverSafePrimesGroupDomainParameterGrain
    {
        private readonly ISafePrimesGroupFactory _safePrimesGroupFactory;
        private SafePrime _param;

        public ObserverSafePrimesGroupDomainParameterGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISafePrimesGroupFactory safePrimesGroupFactory
        ) 
            : base(nonOrleansScheduler)
        {
            _safePrimesGroupFactory = safePrimesGroupFactory;
        }

        public async Task<bool> BeginWorkAsync(SafePrime param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            await Notify(_safePrimesGroupFactory.GetSafePrime(_param));
        }
    }
}