using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Ecdsa;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverEcdsaVerifyKeyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaKeyResult>>, 
        IOracleObserverEcdsaVerifyKeyCaseGrain
    {
        private readonly IEcdsaKeyGenRunner _runner;
        private readonly IEccCurveFactory _curveFactory;
        private readonly IRandom800_90 _rand;

        private EcdsaKeyParameters _param;

        public OracleObserverEcdsaVerifyKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEcdsaKeyGenRunner runner,
            IEccCurveFactory curveFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _runner = runner;
            _curveFactory = curveFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaKeyParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var key = _runner.GenerateKey(_param).Key;
            var curve = _curveFactory.GetCurve(_param.Curve);

            if (_param.Disposition == EcdsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = key.PublicQ;

                do
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                key = new EccKeyPair(modifiedPublicQ, key.PrivateD);
            }
            else if (_param.Disposition == EcdsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = key.PublicQ;

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                key = new EccKeyPair(modifiedPublicQ, key.PrivateD);
            }

            // Notify observers of result
            await Notify(new VerifyResult<EcdsaKeyResult>
            {
                Result = _param.Disposition == EcdsaKeyDisposition.None,
                VerifiedValue = new EcdsaKeyResult
                {
                    Key = key
                }
            });
        }
    }
}