using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Lms;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Lms
{
    public class OracleObserverLmsVerifySignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<LmsSignatureResult>>,
        IOracleObserverLmsVerifySignatureCaseGrain
    {
        private readonly IHssFactory _hssFactory;
        private readonly IRandom800_90 _rand;

        private LmsSignatureParameters _param;
        private LmsKeyResult _key;
        private LmsKeyResult _badKey;

        public OracleObserverLmsVerifySignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IHssFactory hssFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _hssFactory = hssFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(LmsSignatureParameters param, LmsKeyResult key, LmsKeyResult badKey)
        {
            _param = param;
            _key = key;
            _badKey = badKey;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var seed = _rand.GetRandomBitString(256);

            var i = _rand.GetRandomBitString(128);

            var hss = _hssFactory.GetInstance(_param.Layers, _param.LmsTypes, _param.LmotsTypes,
                Math.Entropy.EntropyProviderTypes.Testable, seed, i);

            var message = _rand.GetRandomBitString(1024);

            var signature = await hss.GenerateHssSignatureAsync(message, _key.KeyPair, _param.Advance);

            var sigResult = new LmsSignatureResult
            {
                Message = message,
                PublicKey = _key.KeyPair.PublicKey,
                Signature = signature.Signature,
                I = i,
                SEED = seed
            };

            if (_param.Disposition == LmsSignatureDisposition.ModifyMessage)
            {
                // Generate a different random message
                sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            else if (_param.Disposition == LmsSignatureDisposition.ModifyKey)
            {
                // Generate a different key pair for the test case
                sigResult.PublicKey = _badKey.KeyPair.PublicKey;
            }
            else if (_param.Disposition == LmsSignatureDisposition.ModifySignature)
            {
                var modifiedSignature = _rand.GetDifferentBitStringOfSameSize(signature.Signature);
                sigResult.Signature = modifiedSignature;
            }

            // Notify observers of result
            await Notify(new VerifyResult<LmsSignatureResult>
            {
                Result = _param.Disposition == LmsSignatureDisposition.None,
                VerifiedValue = sigResult
            });
        }
    }
}
