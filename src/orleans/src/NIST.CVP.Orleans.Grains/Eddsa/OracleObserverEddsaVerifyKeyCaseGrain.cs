using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaVerifyKeyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EddsaKeyResult>>, 
        IOracleObserverEddsaVerifyKeyCaseGrain
    {
        private readonly IEddsaKeyGenRunner _runner;
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IRandom800_90 _rand;

        private EddsaKeyParameters _param;

        public OracleObserverEddsaVerifyKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEddsaKeyGenRunner runner,
            IEdwardsCurveFactory curveFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _runner = runner;
            _curveFactory = curveFactory;
            _rand = rand;
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

            if (_param.Disposition == EddsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = curve.Decode(key.PublicQ);

                do
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                key = new EdKeyPair(curve.Encode(modifiedPublicQ), key.PrivateD);
            }
            else if (_param.Disposition == EddsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = curve.Decode(key.PublicQ);

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                key = new EdKeyPair(curve.Encode(modifiedPublicQ), key.PrivateD);
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