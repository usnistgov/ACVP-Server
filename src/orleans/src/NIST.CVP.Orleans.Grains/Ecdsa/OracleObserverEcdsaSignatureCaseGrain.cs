using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaSignatureCaseGrain : ObservableOracleGrainBase<EcdsaSignatureResult>, 
        IOracleObserverEcdsaSignatureCaseGrain
    {

        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IEntropyProvider _entropyProvider;

        private EcdsaSignatureParameters _param;

        public OracleObserverEcdsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);
            var eccDsa = _dsaFactory.GetInstance(_param.HashAlg);

            var message = _entropyProvider.GetEntropy(_param.PreHashedMessage ? _param.HashAlg.OutputLen : 1024);

            var result = eccDsa.Sign(domainParams, _param.Key, message, _param.PreHashedMessage);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EcdsaSignatureResult
            {
                Message = message,
                Signature = result.Signature
            });
        }
    }
}