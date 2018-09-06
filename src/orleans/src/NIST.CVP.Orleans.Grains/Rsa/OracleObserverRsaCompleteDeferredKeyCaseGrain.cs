using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
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
        ) : base (nonOrleansScheduler)
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
            var entropyProvider = _entropyProviderFactory.GetEntropyProvider(
                EntropyProviderTypes.Testable
            );
            _param.PublicExponent = new BitString(_fullParam.Key.PubKey.E);

            if (_param.KeyMode == PrimeGenModes.B32)
            {
                // Nothing
            }
            else if (_param.KeyMode == PrimeGenModes.B33)
            {
                // P and Q
                entropyProvider.AddEntropy(new BitString(_fullParam.Key.PrivKey.P, _param.Modulus / 2));
                entropyProvider.AddEntropy(new BitString(_fullParam.Key.PrivKey.Q, _param.Modulus / 2));
            }
            else if (_param.KeyMode == PrimeGenModes.B34)
            {
                // Nothing
            }
            else if (_param.KeyMode == PrimeGenModes.B35)
            {
                // XP and XQ
                entropyProvider.AddEntropy(_fullParam.AuxValues.XP);
                entropyProvider.AddEntropy(_fullParam.AuxValues.XQ);
            }
            else if (_param.KeyMode == PrimeGenModes.B36)
            {
                // XP and XQ
                entropyProvider.AddEntropy(_fullParam.AuxValues.XP);
                entropyProvider.AddEntropy(_fullParam.AuxValues.XQ);

                // XP1, XP2, XQ1, XQ2
                entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XP1).GetLeastSignificantBits(_fullParam.BitLens[0]));
                entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XP2).GetLeastSignificantBits(_fullParam.BitLens[1]));
                entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XQ1).GetLeastSignificantBits(_fullParam.BitLens[2]));
                entropyProvider.AddEntropy(new BitString(_fullParam.AuxValues.XQ2).GetLeastSignificantBits(_fullParam.BitLens[3]));
            }

            // Notify observers of result
            await Notify(new RsaKeyResult
            {
                Key = _rsaRunner.GeneratePrimes(_param, entropyProvider).Key
            });
        }
    }
}