using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly AesMonteCarloFactory _aesMctFactory = new AesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly CounterFactory _ctrFactory = new CounterFactory();
        
        private AesXtsResult GetAesXtsCase(AesXtsParameters param)
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

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength * 2);
            var i = new BitString(0);
            var number = 0;

            if (param.TweakMode.Equals("hex", StringComparison.OrdinalIgnoreCase))
            {
                i = _rand.GetRandomBitString(128);
            }
            else if (param.TweakMode.Equals("number", StringComparison.OrdinalIgnoreCase))
            {
                number = _rand.GetRandomInt(0, 256);
                i = XtsHelper.GetIFromInteger(number);
            }

            var blockCipherParams = new ModeBlockCipherParameters(direction, i, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            return new AesXtsResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                SequenceNumber = number,
                Iv = i,
                Key = key
            };
        }
        
        public async Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
        
        public async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<AesResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<AesResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            return await _taskFactory.StartNew(() => GetAesXtsCase(param));
        }

        public async Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesDeferredCounterCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCompleteDeferredCounterCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCounterExtractIvsCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<CounterResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<CounterResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        private BitString GetStartingIv(bool overflow, bool incremental)
        {
            BitString padding;

            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            //int randomBits = _isSample ? 6 : 9;
            int randomBits = 6;

            if (overflow == incremental)
            {
                padding = BitString.Ones(128 - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(128 - randomBits);
            }

            return BitString.ConcatenateBits(padding, _rand.GetRandomBitString(randomBits));
        }
    }
}
