using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Orleans.Grains.Aes
{
    public class OracleObserverAesFfCaseGrain : ObservableOracleGrainBase<AesResult>, 
        IOracleObserverAesFfCaseGrain
    {
        
        private readonly IEntropyProvider _entropyProvider;
        private readonly IFfxModeBlockCipherFactory _aesFfxModeBlockCipherFactory;
        
        private AesFfParameters _param;
        
        public OracleObserverAesFfCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IFfxModeBlockCipherFactory aesFfxModeBlockCipherFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _aesFfxModeBlockCipherFactory = aesFfxModeBlockCipherFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(AesFfParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // TODO the actual work...
//            var cipher = _modeFactory.GetStandardCipher(
//                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
//                _param.Mode
//            );
//            var direction = BlockCipherDirections.Encrypt;
//            if (_param.Direction.ToLower() == "decrypt")
//            {
//                direction = BlockCipherDirections.Decrypt;
//            }
//
//            var payload = _entropyProvider.GetEntropy(_param.DataLength);
//            var key = _entropyProvider.GetEntropy(_param.KeyLength);
//            var iv = _entropyProvider.GetEntropy(128);
//
//            var blockCipherParams = new ModeBlockCipherParameters(
//                direction, 
//                iv.GetDeepCopy(), 
//                key.GetDeepCopy(), 
//                payload.GetDeepCopy()
//            );
//            var result = cipher.ProcessPayload(blockCipherParams);
//
//            if (!result.Success)
//            {
//                // Log error somewhere
//                throw new Exception();
//            }
//
//            // Notify observers of result
//            await Notify(new AesResult
//            {
//                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
//                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
//                Key = key,
//                Iv = iv
//            });
        }
    }
}