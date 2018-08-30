using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly CounterFactory _ctrFactory = new CounterFactory();
        private readonly TdesPartitionsMonteCarloFactory _tdesWithIvsMctFactory = new TdesPartitionsMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());

        private TdesResultWithIvs GetTdesWithIvsCase(TdesParameters param)
        {
            var cipher = _modeFactory.GetStandardCipher(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes), 
                param.Mode
            );
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);
            var ivs = TdesPartitionHelpers.SetupIvs(iv);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new TdesResultWithIvs
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv1 = ivs[0],
                Iv2 = ivs[1],
                Iv3 = ivs[2]
            };
        }

        private MctResult<TdesResultWithIvs> GetTdesMctWithIvsCase(TdesParameters param)
        {
            var cipher = _tdesWithIvsMctFactory.GetInstance(param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new MctResult<TdesResultWithIvs>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResultWithIvs
                {
                    Key = element.Keys,
                    Iv1 = element.IV1,
                    Iv2 = element.IV2,
                    Iv3 = element.IV3,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }

        private TdesResult GetDeferredTdesCounterCase(CounterParameters<TdesParameters> param)
        {
            var iv = GetStartingIv(param.Overflow, param.Incremental);

            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.Parameters.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.Parameters.KeyingOption);

            return new TdesResult
            {
                Key = key,
                Iv = iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : null
            };
        }

        private TdesResult CompleteDeferredTdesCounterCase(CounterParameters<TdesParameters> param)
        {
            var fullParam = GetDeferredTdesCounterCase(param);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            var counter = _ctrFactory.GetCounter(
                engine, 
                param.Incremental ? CounterTypes.Additive : CounterTypes.Subtractive, 
                fullParam.Iv
            );
            var cipher = _modeFactory.GetCounterCipher(
                engine, 
                counter
            );

            var blockCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText, null);
  
            var result = cipher.ProcessPayload(blockCipherParams);

            return new TdesResult
            {
                Key = fullParam.Key,
                Iv = fullParam.Iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? fullParam.CipherText : result.Result
            };
        }

        private CounterResult ExtractIvs(TdesParameters param, TdesResult fullParam)
        {
            var cipher = _modeFactory.GetIvExtractor(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes)
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

        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TdesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TdesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<TdesResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<TdesResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<TdesResultWithIvs> GetTdesWithIvsCaseAsync(TdesParameters param)
        {
            return await _taskFactory.StartNew(() => GetTdesWithIvsCase(param));
        }

        public async Task<MctResult<TdesResultWithIvs>> GetTdesMctWithIvsCaseAsync(TdesParameters param)
        {
            return await _taskFactory.StartNew(() => GetTdesMctWithIvsCase(param));
        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            return await _taskFactory.StartNew(() => GetDeferredTdesCounterCase(param));
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredTdesCounterCase(param));
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            return await _taskFactory.StartNew(() => ExtractIvs(param, fullParam));
        }
    }
}
