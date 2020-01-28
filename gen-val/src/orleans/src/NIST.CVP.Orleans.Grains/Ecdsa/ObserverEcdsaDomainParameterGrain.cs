using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class ObserverEcdsaDomainParameterGrain : ObservableOracleGrainBase<EccDomainParameters>, IObserverEcdsaDomainParameterGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        
        private Curve _param;
        
        public ObserverEcdsaDomainParameterGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory
            ) 
            : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
        }

        public async Task<bool> BeginWorkAsync(Curve param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            await Notify(new EccDomainParameters(_curveFactory.GetCurve(_param)));
        }
    }
}