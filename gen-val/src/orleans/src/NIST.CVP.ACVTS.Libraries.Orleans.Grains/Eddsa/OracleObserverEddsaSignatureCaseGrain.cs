using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaSignatureCaseGrain : ObservableOracleGrainBase<EddsaSignatureResult>,
        IOracleObserverEddsaSignatureCaseGrain
    {

        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRandom800_90 _rand;
        private const int BITS_IN_BYTE = 8;
        
        private EddsaSignatureParameters _param;

        public OracleObserverEddsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEdwardsCurveFactory curveFactory,
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);
            var edDsa = _dsaFactory.GetInstance(null);

            var message = _rand.GetRandomBitString(1024);

            // _param.ContextLength will either have been 1) explicitly set to a value or 2) not set to a value. In the 
            // case of #2, _param.ContextLength will default to 0 -- returns "new BitString("")" if input is <= 0
            var context = _rand.GetRandomBitString(_param.ContextLength * BITS_IN_BYTE);

            var result = edDsa.Sign(domainParams, _param.Key, message, context, _param.PreHash);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EddsaSignatureResult
            {
                Message = message,
                Context = context,
                ContextLength = context.BitLength/BITS_IN_BYTE,
                Signature = result.Signature
            });
        }
    }
}
