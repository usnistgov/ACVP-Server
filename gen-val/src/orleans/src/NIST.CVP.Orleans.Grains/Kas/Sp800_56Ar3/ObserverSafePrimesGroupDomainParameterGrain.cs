using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3
{
    public class ObserverSafePrimesGroupDomainParameterGrain : ObservableOracleGrainBase<FfcDomainParametersResult>, IObserverSafePrimesGroupDomainParameterGrain
    {
        private readonly ISafePrimesGroupFactory _safePrimesGroupFactory;
        private SafePrimeParameters _param;

        public ObserverSafePrimesGroupDomainParameterGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ISafePrimesGroupFactory safePrimesGroupFactory
        ) 
            : base(nonOrleansScheduler)
        {
            _safePrimesGroupFactory = safePrimesGroupFactory;
        }

        public async Task<bool> BeginWorkAsync(SafePrimeParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            await Notify(new FfcDomainParametersResult(){ DomainParameters = _safePrimesGroupFactory.GetSafePrime(_param.SafePrime)});
        }
    }
}