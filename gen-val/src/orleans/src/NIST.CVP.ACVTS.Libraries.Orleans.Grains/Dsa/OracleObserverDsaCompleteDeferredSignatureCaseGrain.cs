using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
{
    public class OracleObserverDsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<DsaSignatureResult>>,
        IOracleObserverDsaCompleteDeferredSignatureCaseGrain
    {

        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;


        private DsaSignatureParameters _param;
        private DsaSignatureResult _fullParam;

        public OracleObserverDsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _dsaFactory = dsaFactory;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public async Task<bool> BeginWorkAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var messageCopy = _fullParam.Message.GetDeepCopy();

            if (_param.IsMessageRandomized)
            {
                var entropyProvider = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(_fullParam.RandomValue);
                messageCopy = _messageRandomizer.WithEntropyProvider(entropyProvider).Build()
                    .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
            }

            var ffcDsa = _dsaFactory.GetInstance(_param.HashAlg);
            var verifyResult = ffcDsa.Verify(_param.DomainParameters, _fullParam.Key, messageCopy, _fullParam.Signature);

            // Notify observers of result
            await Notify(new VerifyResult<DsaSignatureResult>
            {
                Result = verifyResult.Success,
                VerifiedValue = _fullParam
            });
        }
    }
}
