using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.SP800_106;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
{
    public class OracleObserverDsaSignatureCaseGrain : ObservableOracleGrainBase<DsaSignatureResult>,
        IOracleObserverDsaSignatureCaseGrain
    {

        private readonly IDsaFfcFactory _dsaFfcFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IRandom800_90 _rand;

        private DsaSignatureParameters _param;

        public OracleObserverDsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFfcFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _dsaFfcFactory = dsaFfcFactory;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(DsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);
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

            var ffcDsa = _dsaFfcFactory.GetInstance(_param.HashAlg);
            var sigResult = ffcDsa.Sign(_param.DomainParameters, _param.Key, messageCopy);
            if (!sigResult.Success)
            {
                throw new Exception();
            }

            var result = new DsaSignatureResult
            {
                Message = message,
                RandomValue = randomValue,
                Signature = sigResult.Signature,
                Key = _param.Key
            };

            if (_param.Disposition == DsaSignatureDisposition.None)
            {
                await Notify(result);
                return;
            }

            // Modify message
            if (_param.Disposition == DsaSignatureDisposition.ModifyMessage)
            {
                result.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            // Modify public key
            else if (_param.Disposition == DsaSignatureDisposition.ModifyKey)
            {
                var x = result.Key.PrivateKeyX;
                var y = result.Key.PublicKeyY + 2;
                result.Key = new FfcKeyPair(x, y);
            }
            // Modify r
            else if (_param.Disposition == DsaSignatureDisposition.ModifyR)
            {
                var s = result.Signature.S;
                var r = result.Signature.R + 2;
                result.Signature = new FfcSignature(s, r);
            }
            // Modify s
            else if (_param.Disposition == DsaSignatureDisposition.ModifyS)
            {
                var s = result.Signature.S + 2;
                var r = result.Signature.R;
                result.Signature = new FfcSignature(s, r);
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}
