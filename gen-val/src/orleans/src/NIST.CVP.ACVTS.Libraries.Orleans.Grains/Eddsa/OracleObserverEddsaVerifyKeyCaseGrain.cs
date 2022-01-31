using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaVerifyKeyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EddsaKeyResult>>,
        IOracleObserverEddsaVerifyKeyCaseGrain
    {
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaEdFactory;
        private readonly IEddsaKeyGenRunner _runner;
        private readonly IShaFactory _shaFactory;

        private EddsaKeyParameters _param;

        public OracleObserverEddsaVerifyKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEddsaKeyGenRunner runner,
            IEdwardsCurveFactory curveFactory,
            IShaFactory shaFactory,
            IDsaEdFactory dsaEdFactory) : base(nonOrleansScheduler)
        {
            _runner = runner;
            _curveFactory = curveFactory;
            _shaFactory = shaFactory;
            _dsaEdFactory = dsaEdFactory;
        }

        public async Task<bool> BeginWorkAsync(EddsaKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var key = _runner.GenerateKey(_param).Key;
            var curve = _curveFactory.GetCurve(_param.Curve);

            var domainParams = new EdDomainParameters(curve, _shaFactory);

            if (_param.Disposition == EddsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = curve.Decode(key.PublicQ);

                do
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X, modifiedPublicQ.Y + 1);
                    key = new EdKeyPair(curve.Encode(modifiedPublicQ), key.PrivateD);
                } while (_dsaEdFactory.GetInstance(null).ValidateKeyPair(domainParams, key).Success);
            }

            // Notify observers of result
            await Notify(new VerifyResult<EddsaKeyResult>
            {
                Result = _param.Disposition == EddsaKeyDisposition.None,
                VerifiedValue = new EddsaKeyResult
                {
                    Key = key
                }
            });
        }
    }
}
