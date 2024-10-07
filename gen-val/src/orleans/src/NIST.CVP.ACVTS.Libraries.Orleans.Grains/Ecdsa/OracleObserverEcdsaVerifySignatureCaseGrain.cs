using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaVerifySignatureCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaSignatureResult>>,
        IOracleObserverEcdsaVerifySignatureCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IRandom800_90 _rand;

        private EcdsaSignatureParameters _param;
        private EcdsaKeyResult _key;
        private EcdsaKeyResult _badKey;

        public OracleObserverEcdsaVerifySignatureCaseCaseGrain(
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
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param, EcdsaKeyResult key, EcdsaKeyResult badKey)
        {
            _param = param;
            _key = key;
            _badKey = badKey;

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

                var message = _rand.GetRandomBitString(1024);
             
                var messageCopy = message.GetDeepCopy();

                BitString randomValue = null;
                if (_param.IsMessageRandomized)
                {
                    randomValue = _rand.GetRandomBitString(_param.HashAlg.OutputLen);
                    var entropyProvider = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                    entropyProvider.AddEntropy(randomValue);
                    messageCopy = _messageRandomizer.WithEntropyProvider(entropyProvider).Build()
                        .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
                }

                var result = eccDsa.Sign(domainParams, _key.Key, messageCopy);
                if (!result.Success)
                {
                    throw new Exception();
                }

                var sigResult = new EcdsaSignatureResult
                {
                    Message = message,
                    RandomValue = randomValue,
                    Key = _key.Key,
                    Signature = result.Signature
                };

                if (_param.Disposition == EcdsaSignatureDisposition.ModifyMessage)
                {
                    // Generate a different random message
                    sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
                }
                else if (_param.Disposition == EcdsaSignatureDisposition.ModifyKey)
                {
                    // Generate a different key pair for the test case
                    sigResult.Key = _badKey.Key;
                }
                else if (_param.Disposition == EcdsaSignatureDisposition.ModifyR)
                {
                    var modifiedRSignature = new EccSignature(sigResult.Signature.R + 1, sigResult.Signature.S);
                    sigResult.Signature = modifiedRSignature;
                }
                else if (_param.Disposition == EcdsaSignatureDisposition.ModifyS)
                {
                    var modifiedSSignature = new EccSignature(sigResult.Signature.R, sigResult.Signature.S + 1);
                    sigResult.Signature = modifiedSSignature;
                }
                else if (_param.Disposition == EcdsaSignatureDisposition.ZeroR)
                {
                    var modifiedRSignature = new EccSignature(0, sigResult.Signature.S);
                    sigResult.Signature = modifiedRSignature;
                }
                else if (_param.Disposition == EcdsaSignatureDisposition.ZeroS)
                {
                    var modifiedSSignature = new EccSignature(sigResult.Signature.R, 0);
                    sigResult.Signature = modifiedSSignature;
                }
                
                // Notify observers of result
                await Notify(new VerifyResult<EcdsaSignatureResult>
                {
                    Result = _param.Disposition == EcdsaSignatureDisposition.None,
                    VerifiedValue = sigResult
                });
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
