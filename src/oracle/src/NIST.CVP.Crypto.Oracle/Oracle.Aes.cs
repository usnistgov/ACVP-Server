using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly AesMonteCarloFactory _aesMctFactory = new AesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly CounterFactory _ctrFactory = new CounterFactory();
        
        private AesResult GetDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            var iv = GetStartingIv(param.Overflow, param.Incremental);

            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.Parameters.DataLength);
            var key = _rand.GetRandomBitString(param.Parameters.KeyLength);

            return new AesResult
            {
                Key = key,
                Iv = iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : null
            };
        }

        private AesResult CompleteDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            var fullParam = GetDeferredAesCounterCase(param);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            var counter = _ctrFactory.GetCounter(
                engine, 
                param.Incremental ? CounterTypes.Additive : CounterTypes.Subtractive, fullParam.Iv
            );
            var cipher = _modeFactory.GetCounterCipher(
                engine, 
                counter
            );

            var blockCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText, null);
  
            var result = cipher.ProcessPayload(blockCipherParams);

            return new AesResult
            {
                Key = fullParam.Key,
                Iv = fullParam.Iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? fullParam.CipherText : result.Result
            };
        }

        private CounterResult ExtractIvs(AesParameters param, AesResult fullParam)
        {
            var cipher = _modeFactory.GetIvExtractor(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes)
            );
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText;
            var result = direction == BlockCipherDirections.Encrypt ? fullParam.CipherText : fullParam.PlainText;

            var counterCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, payload, result);

            var extractedIvs = cipher.ExtractIvs(counterCipherParams);

            if (!extractedIvs.Success)
            {
                // TODO log error somewhere
                throw new Exception();
            }

            return new CounterResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? null : extractedIvs.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? null : extractedIvs.Result,
                IVs = extractedIvs.IVs
            };
        }

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
            //var grain = _clusterClient.GetGrain<IOracleAesCaseGrain>(
            //    Guid.NewGuid()
            //);

            //await grain.BeginWorkAsync(param);
            //return await PollWorkUntilCompleteAsync(grain);

            var grain = _clusterClient.GetGrain<IOracleObserverAesCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AesResult>();
            var observerReference = await _clusterClient.CreateObjectReference<IGrainObserver<AesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            //var cancellationTokenSource = new CancellationTokenSource();
            //var subscriptionTask = StaySubscribed(grain, observerReference, cancellationTokenSource.Token);

            while (!observer.HasResult)
            {
                //await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds), cancellationTokenSource.Token);
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await grain.Subscribe(observerReference);
            }

            var result = observer.GetResult();
            await grain.Unsubscribe(observerReference);
            //cancellationTokenSource.Cancel();

            return result;
        }

        //private static async Task StaySubscribed(IOracleObserverAesCaseGrain grain, IGrainObserver<AesResult> observer, CancellationToken token)
        //{
        //    while (!token.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            await Task.Delay(TimeSpan.FromSeconds(5), token);
        //            await grain.Subscribe(observer);
        //        }
        //        catch (Exception exception)
        //        {
        //            Console.WriteLine($"Exception while trying to subscribe for updates: {exception}");
        //        }
        //    }
        //}

        public async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleAesMctCaseGrain>(
                Guid.NewGuid()
            );

            await grain.BeginWorkAsync(param);
            return await PollWorkUntilCompleteAsync(grain);
        }

        public async Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            return await _taskFactory.StartNew(() => GetAesXtsCase(param));
        }

        public async Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            return await _taskFactory.StartNew(() => GetDeferredAesCounterCase(param));
        }

        public async Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredAesCounterCase(param));
        }

        public async Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            return await _taskFactory.StartNew(() => ExtractIvs(param, fullParam));
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
