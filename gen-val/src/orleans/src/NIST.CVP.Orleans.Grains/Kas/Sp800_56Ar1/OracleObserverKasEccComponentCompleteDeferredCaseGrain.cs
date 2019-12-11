﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
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
        ) : base (nonOrleansScheduler)
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