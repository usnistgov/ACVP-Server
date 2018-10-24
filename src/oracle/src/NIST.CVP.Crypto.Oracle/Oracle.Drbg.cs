using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly DrbgFactory _drbgFactory = new DrbgFactory();
        private const string DRBG_INTENDED_USE_GENERATE = "generate";
        private const string DRBG_INTENDED_USE_reSeed = "reSeed";

        private DrbgResult GetNoReseedCase(DrbgParameters param)
        {
            var rand = new Random800_90();
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = param.PredResistanceEnabled ? rand.GetRandomBitString(param.EntropyInputLen) : new BitString(0),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = param.PredResistanceEnabled ? rand.GetRandomBitString(param.EntropyInputLen) : new BitString(0),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                }
            };

            var entropyInput = rand.GetRandomBitString(param.EntropyInputLen);
            var nonce = rand.GetRandomBitString(param.NonceLen);
            var persoString = rand.GetRandomBitString(param.PersoStringLen);

            var testableEntropy = new TestableEntropyProvider();
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
            var rand = new Random800_90();
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_reSeed,
                    EntropyInput = rand.GetRandomBitString(param.EntropyInputLen),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = new BitString(0),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = new BitString(0),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                }
            };

            var entropyInput = rand.GetRandomBitString(param.EntropyInputLen);
            var nonce = rand.GetRandomBitString(param.NonceLen);
            var persoString = rand.GetRandomBitString(param.PersoStringLen);

            var testableEntropy = new TestableEntropyProvider();
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
            var rand = new Random800_90();
            var otherInput = new List<OtherInput>
            {
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = rand.GetRandomBitString(param.EntropyInputLen),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                },
                new OtherInput
                {
                    IntendedUse = DRBG_INTENDED_USE_GENERATE,
                    EntropyInput = rand.GetRandomBitString(param.EntropyInputLen),
                    AdditionalInput = rand.GetRandomBitString(param.AdditionalInputLen)
                }
            };

            var entropyInput = rand.GetRandomBitString(param.EntropyInputLen);
            var nonce = rand.GetRandomBitString(param.NonceLen);
            var persoString = rand.GetRandomBitString(param.PersoStringLen);

            var testableEntropy = new TestableEntropyProvider();
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

        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            return await _taskFactory.StartNew(() => GetDrbgCase(param));
        }
    }
}
