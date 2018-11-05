using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaVerifyKeyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaKeyResult>>, 
        IOracleObserverEcdsaVerifyKeyCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IRandom800_90 _rand;

        private EcdsaKeyParameters _param;
        private EcdsaKeyResult _key;

        public OracleObserverEcdsaVerifyKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaKeyParameters param, EcdsaKeyResult key)
        {
            _param = param;
            _key = key;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);

            if (_param.Disposition == EcdsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = _key.Key.PublicQ;

                do
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                _key.Key = new EccKeyPair(modifiedPublicQ, _key.Key.PrivateD);
            }
            else if (_param.Disposition == EcdsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = _key.Key.PublicQ;

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                _key.Key = new EccKeyPair(modifiedPublicQ, _key.Key.PrivateD);
            }

            // Notify observers of result
            await Notify(new VerifyResult<EcdsaKeyResult>
            {
                Result = _param.Disposition == EcdsaKeyDisposition.None,
                VerifiedValue = new EcdsaKeyResult
                {
                    Key = _key.Key
                }
            });
        }
    }
}