﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Orleans.Grains.Aead;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverAesCompleteDeferredXpnCaseGrain : ObservableOracleGrainBase<AeadResult>, 
        IOracleObserverAesCompleteDeferredXpnCaseGrain
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        
        private AeadParameters _param;
        private AeadResult _fullParam;

        public OracleObserverAesCompleteDeferredXpnCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory
        ) : base (nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
        }
        
        public async Task<bool> BeginWorkAsync(AeadParameters param, AeadResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var testParam = new AeadResult
            {
                Aad = _fullParam.Aad,
                CipherText = _fullParam.CipherText,
                Iv = _fullParam.Iv.XOR(_fullParam.Salt),
                Key = _fullParam.Key,
                PlainText = _fullParam.PlainText,
                Tag = _fullParam.Tag
            };

            var result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                testParam, 
                _param
            );

            // Notify observers of result
            await Notify(result);
        }
    }
}