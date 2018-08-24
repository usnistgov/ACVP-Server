using System;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;
using Orleans;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverAesCaseGrain : Grain, IOracleObserverAesCaseGrain
    {
        private GrainObserverManager<IGrainObserver<AesResult>> _subsManager;
        private readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IEntropyProvider _entropyProvider;

        public OracleObserverAesCaseGrain(
            LimitedConcurrencyLevelTaskScheduler scheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            IEntropyProviderFactory entropyProviderFactory)
        {
            _scheduler = scheduler;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public override async Task OnActivateAsync()
        {
            // We created the utility at activation time.
            _subsManager = new GrainObserverManager<IGrainObserver<AesResult>>();
            await base.OnActivateAsync();
        }

        public Task Subscribe(IGrainObserver<AesResult> observer)
        {
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        public Task Unsubscribe(IGrainObserver<AesResult> observer)
        {
            _subsManager.Unsubscribe(observer);
            return Task.CompletedTask;
        }

        public Task<bool> BeginWorkAsync(AesParameters param)
        {
            var orleansTaskScheduler = TaskScheduler.Current;

            Task.Factory.StartNew(() =>
            {
                DoWorkAsync(param, orleansTaskScheduler).FireAndForget();
            }, CancellationToken.None, TaskCreationOptions.None, scheduler: _scheduler).FireAndForget();

            return Task.FromResult(true);
        }

        protected async Task DoWorkAsync(AesParameters param, TaskScheduler orleansTaskScheduler)
        {
            var cipher = _modeFactory.GetStandardCipher(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                param.Mode
            );
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _entropyProvider.GetEntropy(param.DataLength);
            var key = _entropyProvider.GetEntropy(param.KeyLength);
            var iv = _entropyProvider.GetEntropy(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            var r = new AesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            };

            // notify
            await Notify(r, orleansTaskScheduler);
        }

        private async Task Notify(AesResult result, TaskScheduler orleansTaskScheduler)
        {
            //_subsManager.Notify(observer => observer.ReceiveMessageFromCluster(result));
            await Task.Factory.StartNew(() =>
            {
                _subsManager.Notify(observer => observer.ReceiveMessageFromCluster(result));
            }, CancellationToken.None, TaskCreationOptions.None, scheduler: orleansTaskScheduler);
        }
    }
}