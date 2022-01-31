using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1
{
    public class OracleObserverKasEccComponentCompleteDeferredCaseGrain : ObservableOracleGrainBase<KasEccComponentDeferredResult>,
        IOracleObserverKasEccComponentCompleteDeferredCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IEccDhComponent _diffieHellman;

        private KasEccComponentDeferredParameters _param;

        public OracleObserverKasEccComponentCompleteDeferredCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IEccDhComponent diffieHellman
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _diffieHellman = diffieHellman;
        }

        public async Task<bool> BeginWorkAsync(KasEccComponentDeferredParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParameters = new EccDomainParameters(curve);

            // Notify observers of result
            await Notify(new KasEccComponentDeferredResult()
            {
                Z = _diffieHellman.GenerateSharedSecret(
                        domainParameters,
                        new EccKeyPair(_param.PrivateKeyServer),
                        new EccKeyPair(new EccPoint(_param.PublicKeyIutX, _param.PublicKeyIutY))
                    ).SharedSecretZ
            });
        }
    }
}
