using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverDrbgCaseGrain : ObservableOracleGrainBase<DrbgResult>, 
        IOracleObserverDrbgCaseGrain
    {
        private const string DRBG_ITENDED_USE_GENERATE = "generate";
        private const string DRBG_ITENDED_USE_reSeed = "reSeed";

        private readonly IDrbgFactory _drbgFactory;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IEntropyProvider _randomEntropy;
        
        private DrbgParameters _param;

        public OracleObserverDrbgCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDrbgFactory drbgFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _drbgFactory = drbgFactory;
            _entropyProviderFactory = entropyProviderFactory;
            _randomEntropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(DrbgParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var result = GetDrbgCase(_param);

            // Notify observers of result
            await Notify(result);
        }

        private DrbgResult GetNoReseedCase(DrbgParameters param)
        {
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = param.PredResistanceEnabled ? _randomEntropy.GetEntropy(param.EntropyInputLen) : new BitString(0),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = param.PredResistanceEnabled ? _randomEntropy.GetEntropy(param.EntropyInputLen) : new BitString(0),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                }
            };

            var entropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen);
            var nonce = _randomEntropy.GetEntropy(param.NonceLen);
            var persoString = _randomEntropy.GetEntropy(param.PersoStringLen);

            var testableEntropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
            testableEntropy.AddEntropy(entropyInput);
            testableEntropy.AddEntropy(nonce);
            otherInput.ForEach(oi => testableEntropy.AddEntropy(oi.EntropyInput));

            var drbg = _drbgFactory.GetDrbgInstance(param, testableEntropy);
            drbg.Instantiate(param.SecurityStrength, persoString);

            var fullResult = new DrbgResult
            {
                EntropyInput = entropyInput,
                Nonce = nonce,
                PersoString = persoString,
                OtherInput = otherInput
            };

            foreach (var item in otherInput)
            {
                var result = drbg.Generate(param.ReturnedBitsLen, item.AdditionalInput);
                if (!result.Success)
                {
                    fullResult.Status = result.DrbgStatus;
                    return fullResult;
                }

                fullResult.ReturnedBits = result.Bits.GetDeepCopy();
            }

            return fullResult;
        }

        private DrbgResult GetReseedNoPredResistCase(DrbgParameters param)
        {
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_reSeed,
                    EntropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = new BitString(0),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = new BitString(0),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                }
            };

            var entropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen);
            var nonce = _randomEntropy.GetEntropy(param.NonceLen);
            var persoString = _randomEntropy.GetEntropy(param.PersoStringLen);

            var testableEntropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
            testableEntropy.AddEntropy(entropyInput);
            testableEntropy.AddEntropy(nonce);
            otherInput.ForEach(oi => testableEntropy.AddEntropy(oi.EntropyInput));

            var drbg = _drbgFactory.GetDrbgInstance(param, testableEntropy);
            drbg.Instantiate(param.SecurityStrength, persoString);

            var fullResult = new DrbgResult
            {
                EntropyInput = entropyInput,
                Nonce = nonce,
                PersoString = persoString,
                OtherInput = otherInput
            };

            var needsReseed = true;
            foreach (var item in otherInput)
            {
                if (needsReseed)
                {
                    needsReseed = false;
                    var reseed = drbg.Reseed(item.AdditionalInput);

                    if (reseed != DrbgStatus.Success)
                    {
                        fullResult.Status = reseed;
                        return fullResult;
                    }

                    continue;
                }

                var result = drbg.Generate(param.ReturnedBitsLen, item.AdditionalInput);
                if (!result.Success)
                {
                    fullResult.Status = result.DrbgStatus;
                    return fullResult;
                }

                fullResult.ReturnedBits = result.Bits.GetDeepCopy();
            }

            return fullResult;
        }

        private DrbgResult GetReseedPredResistCase(DrbgParameters param)
        {
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_ITENDED_USE_GENERATE,
                    EntropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen),
                    AdditionalInput = _randomEntropy.GetEntropy(param.AdditionalInputLen)
                }
            };

            var entropyInput = _randomEntropy.GetEntropy(param.EntropyInputLen);
            var nonce = _randomEntropy.GetEntropy(param.NonceLen);
            var persoString = _randomEntropy.GetEntropy(param.PersoStringLen);

            var testableEntropy = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
            testableEntropy.AddEntropy(entropyInput);
            testableEntropy.AddEntropy(nonce);
            otherInput.ForEach(oi => testableEntropy.AddEntropy(oi.EntropyInput));

            var drbg = _drbgFactory.GetDrbgInstance(param, testableEntropy);
            drbg.Instantiate(param.SecurityStrength, persoString);

            var fullResult = new DrbgResult
            {
                EntropyInput = entropyInput,
                Nonce = nonce,
                PersoString = persoString,
                OtherInput = otherInput
            };

            foreach (var item in otherInput)
            {
                var result = drbg.Generate(param.ReturnedBitsLen, item.AdditionalInput);
                if (!result.Success)
                {
                    fullResult.Status = result.DrbgStatus;
                    return fullResult;
                }

                fullResult.ReturnedBits = result.Bits.GetDeepCopy();
            }

            return fullResult;
        }

        private DrbgResult GetDrbgCase(DrbgParameters param)
        {
            if (!param.ReseedImplemented)
            {
                return GetNoReseedCase(param);
            }
            else
            {
                if (param.PredResistanceEnabled)
                {
                    return GetReseedPredResistCase(param);
                }
                else
                {
                    return GetReseedNoPredResistCase(param);
                }
            }
        }
    }
}