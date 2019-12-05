using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;

namespace NIST.CVP.Orleans.Grains.Aead
{
    public class OracleObserverAesXpnCaseGrain : ObservableOracleGrainBase<AeadResult>, 
        IOracleObserverAesXpnCaseGrain
    {
        private const double FAIL_RATE = 0.25;
        
        private readonly IRandom800_90 _rand;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        
        private AeadParameters _param;

        public OracleObserverAesXpnCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IRandom800_90 rand,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _rand = rand;
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
        }
        
        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(_param.PayloadLength),
                Key = _rand.GetRandomBitString(_param.KeyLength),
                Salt = _rand.GetRandomBitString(_param.SaltLength),
                Iv = _rand.GetRandomBitString(_param.IvLength),
                Aad = _rand.GetRandomBitString(_param.AadLength)
            };

            var tempParams = new AeadResult
            {
                PlainText = fullParams.PlainText,
                Key = fullParams.Key,
                Iv = fullParams.Iv.XOR(fullParams.Salt),
                Aad = fullParams.Aad
            };

            // Uses gcm as a cipher instead of xpn
            var result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                tempParams, 
                _param
            );

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / FAIL_RATE);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                    result.TestPassed = false;
                }
            }

            result.Iv = fullParams.Iv;
            result.Salt = fullParams.Salt;

            // Notify observers of result
            await Notify(result);
        }
    }
}