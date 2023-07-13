using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaAlterKeyCaseGrain : ObservableOracleGrainBase<EcdsaKeyResult>,
        IOracleObserverEcdsaAlterKeyCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IRandom800_90 _rand;

        private EcdsaAlterKeyParameters _param;
        private EccKeyPair _key;

        public OracleObserverEcdsaAlterKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(EcdsaAlterKeyParameters param)
        {
            _param = param;
            _key = param.Key;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);

            if (_param.Disposition == EcdsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = _key.PublicQ;

                do
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                _key = new EccKeyPair(modifiedPublicQ, _key.PrivateD);
            }
            else if (_param.Disposition == EcdsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = _key.PublicQ;

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                _key = new EccKeyPair(modifiedPublicQ, _key.PrivateD);
            }

            // Notify observers of result
            await Notify(new EcdsaKeyResult
            {
                Key = _key
            });
        }
    }
}
