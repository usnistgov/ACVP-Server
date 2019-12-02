using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Aes;

namespace NIST.CVP.Orleans.Grains.Aes
{
    public class OracleObserverAesMctCaseGrain : ObservableOracleGrainBase<MctResult<AesResult>>, 
        IOracleObserverAesMctCaseGrain
    {
        private readonly IMonteCarloFactoryAes _mctFactory;
        private readonly IEntropyProvider _entropyProvider;

        private AesParameters _param;

        public OracleObserverAesMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IMonteCarloFactoryAes mctFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _mctFactory = mctFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(AesParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var cipher = _mctFactory.GetInstance(_param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _entropyProvider.GetEntropy(_param.DataLength);
            var key = _entropyProvider.GetEntropy(_param.KeyLength);
            var iv = _entropyProvider.GetEntropy(128);

            var blockCipherParams = new ModeBlockCipherParameters(
                direction, 
                iv.GetDeepCopy(), 
                key.GetDeepCopy(), 
                payload.GetDeepCopy()
            );
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new MctResult<AesResult>
            {
                Seed = new AesResult()
                {
                    Key = key,
                    Iv = iv,
                    PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                    CipherText = direction == BlockCipherDirections.Decrypt ? payload : null,
                },
                Results = Array.ConvertAll(result.Response.ToArray(), element => new AesResult
                {
                    Key = element.Key,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            });
        }
    }
}