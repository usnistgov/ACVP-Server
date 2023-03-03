using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaCompleteDeferredKeyCaseGrain : ObservableOracleGrainBase<RsaKeyResult>,
        IOracleObserverRsaCompleteDeferredKeyCaseGrain
    {
        private readonly IRsaRunner _rsaRunner;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        private RsaKeyParameters _param;
        private RsaKeyResult _fullParam;

        public OracleObserverRsaCompleteDeferredKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsaRunner rsaRunner,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _rsaRunner = rsaRunner;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public async Task<bool> BeginWorkAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                var entropyProvider = _entropyProviderFactory.GetEntropyProvider(
                    EntropyProviderTypes.Testable
                );
                _param.PublicExponent = new BitString(_fullParam.Key.PubKey.E);

                if (_param.KeyMode == PrimeGenModes.RandomProvablePrimes)
                {
                    // Nothing
                }
                else if (_param.KeyMode == PrimeGenModes.RandomProbablePrimes)
                {
                    // P and Q
                    entropyProvider.AddEntropy(new BitString(_fullParam.Key.PrivKey.P, _param.Modulus / 2));
                    entropyProvider.AddEntropy(new BitString(_fullParam.Key.PrivKey.Q, _param.Modulus / 2));
                }
                else if (_param.KeyMode == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                {
                    // Nothing
                }
                else if (_param.KeyMode == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes)
                {
                    // XP and XQ
                    entropyProvider.AddEntropy(_fullParam.AuxValues.XP);
                    entropyProvider.AddEntropy(_fullParam.AuxValues.XQ);

                    // RsaRunner needs these values from the fullParam set
                    _param.BitLens = _fullParam.BitLens;
                }
                else if (_param.KeyMode == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes)
                {
                    // XP and XQ
                    entropyProvider.AddEntropy(_fullParam.AuxValues.XP);
                    entropyProvider.AddEntropy(_fullParam.AuxValues.XQ);

                    // XP1, XP2, XQ1, XQ2
                    entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XP1).GetLeastSignificantBits(_fullParam.BitLens[0]));
                    entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XP2).GetLeastSignificantBits(_fullParam.BitLens[1]));
                    entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XQ1).GetLeastSignificantBits(_fullParam.BitLens[2]));
                    entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XQ2).GetLeastSignificantBits(_fullParam.BitLens[3]));

                    // RsaRunner needs these values from the fullParam set
                    _param.BitLens = _fullParam.BitLens;
                }

                // Notify observers of result
                var result = _rsaRunner.GeneratePrimes(_param, entropyProvider);

                if (!result.Success)
                {
                    await Notify(new RsaKeyResult { ErrorMessage = result.ErrorMessage });   
                }
                else
                {
                    await Notify(new RsaKeyResult { Key = result.Key });    
                }
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}
