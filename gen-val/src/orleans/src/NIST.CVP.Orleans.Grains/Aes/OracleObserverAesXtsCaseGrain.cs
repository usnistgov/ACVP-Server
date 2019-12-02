using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Orleans.Grains.Aes
{
    public class OracleObserverAesXtsCaseGrain : ObservableOracleGrainBase<AesXtsResult>, 
        IOracleObserverAesXtsCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IRandom800_90 _rand;

        private AesXtsParameters _param;

        public OracleObserverAesXtsCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(AesXtsParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetStandardCipher(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                _param.Mode
            );
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(_param.DataLength);
            var key = _rand.GetRandomBitString(_param.KeyLength * 2);
            var i = new BitString(0);
            var number = 0;

            if (_param.TweakMode.Equals("hex", StringComparison.OrdinalIgnoreCase))
            {
                i = _rand.GetRandomBitString(128);
            }
            else if (_param.TweakMode.Equals("number", StringComparison.OrdinalIgnoreCase))
            {
                number = _rand.GetRandomInt(0, 256);
                i = XtsHelper.GetIFromInteger(number);
            }

            var blockCipherParams = new ModeBlockCipherParameters(direction, i, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            // Notify observers of result
            await Notify(new AesXtsResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                SequenceNumber = number,
                Iv = i,
                Key = key
            });
        }
    }
}