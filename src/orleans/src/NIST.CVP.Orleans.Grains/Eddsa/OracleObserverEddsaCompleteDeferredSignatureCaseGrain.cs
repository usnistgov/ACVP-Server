﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<EddsaSignatureResult>>, 
        IOracleObserverEddsaCompleteDeferredSignatureCaseGrain
    {
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        private EddsaSignatureParameters _param;
        private EddsaSignatureResult _fullParam;

        public OracleObserverEddsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEdwardsCurveFactory curveFactory,
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var edDsa = _dsaFactory.GetInstance(null);
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);

            var result = edDsa.Verify(domainParams, _param.Key, _fullParam.Message, _fullParam.Signature, _fullParam.Context, _param.PreHash);

            // Notify observers of result
            await Notify(new VerifyResult<EddsaSignatureResult>
            {
                Result = result.Success
            });
        }
    }
}