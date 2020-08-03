using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaSignatureCaseGrain : ObservableOracleGrainBase<EcdsaSignatureResult>,
        IOracleObserverEcdsaSignatureCaseGrain
    {

        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private EcdsaSignatureParameters _param;

        public OracleObserverEcdsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var curve = _curveFactory.GetCurve(_param.Curve);
                var domainParams = new EccDomainParameters(curve);
                var eccDsa = _dsaFactory.GetInstanceForSignatures(_param.HashAlg, _param.NonceProviderType, new EntropyProvider(new Random800_90()));

                var message = _entropyProvider.GetEntropy(_param.PreHashedMessage ? _param.HashAlg.OutputLen : 1024);

                BitString randomValue = null;
                var messageCopy = message.GetDeepCopy();
                if (_param.IsMessageRandomized)
                {
                    randomValue = _rand.GetRandomBitString(_param.HashAlg.OutputLen);
                    var entropyProvider = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                    entropyProvider.AddEntropy(randomValue);
                    messageCopy = _messageRandomizer.WithEntropyProvider(entropyProvider).Build()
                        .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
                }

                var result = eccDsa.Sign(domainParams, _param.Key, messageCopy, _param.PreHashedMessage);
                if (!result.Success)
                {
                    throw new Exception();
                }

                // Notify observers of result
                await Notify(new EcdsaSignatureResult
                {
                    Message = message,
                    RandomValue = randomValue,
                    Signature = result.Signature
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}