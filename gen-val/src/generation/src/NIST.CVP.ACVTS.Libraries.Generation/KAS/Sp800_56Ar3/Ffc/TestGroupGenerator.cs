using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, FfcDomainParameters, FfcKeyPair>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        protected override async Task GenerateDomainParametersAsync(List<TestGroup> groups)
        {
            var dpModes = groups.Select(s => s.DomainParameterGenerationMode).Distinct();
            var dpDict = new Dictionary<KasDpGeneration, Task<FfcDomainParameters>>();

            foreach (var mode in dpModes)
            {
                dpDict.Add(mode, GenerateDomainParametersAsync(mode));
            }

            await Task.WhenAll(dpDict.Values);

            foreach (var group in groups)
            {
                group.DomainParameters = await dpDict.First(w => w.Key == group.DomainParameterGenerationMode).Value;
            }
        }

        protected override async Task GenerateKeysPerDomainParametersAsync(List<TestGroup> groupList)
        {
            var dps = groupList.Select(s => new
            {
                s.DomainParameterGenerationMode,
                s.FfcDomainParameters
            }).Distinct();

            var tasks = new Dictionary<KasDpGeneration, List<Task<FfcKeyPair>>>();

            foreach (var dp in dps)
            {
                var list = new List<Task<FfcKeyPair>>();
                for (var i = 0; i < 100; i++)
                {
                    list.Add(GetKey(dp.FfcDomainParameters));
                }

                tasks.Add(dp.DomainParameterGenerationMode, list);
            }

            await Task.WhenAll(tasks.SelectMany(s => s.Value));

            var shuffleQueues = new Dictionary<KasDpGeneration, ShuffleQueue<FfcKeyPair>>();
            foreach (var kvp in tasks)
            {
                List<FfcKeyPair> keys = new List<FfcKeyPair>();
                foreach (var task in kvp.Value)
                {
                    keys.Add(await task);
                }
                shuffleQueues.Add(kvp.Key, new ShuffleQueue<FfcKeyPair>(keys));
            }

            foreach (var group in groupList)
            {
                group.ShuffleKeys = shuffleQueues.First(f => f.Key == group.DomainParameterGenerationMode).Value;
            }
        }

        private async Task<FfcDomainParameters> GenerateDomainParametersAsync(KasDpGeneration domainParameterGenerationMode)
        {
            return domainParameterGenerationMode switch
            {
                KasDpGeneration.Ffdhe2048 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Ffdhe2048),
                KasDpGeneration.Ffdhe3072 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Ffdhe3072),
                KasDpGeneration.Ffdhe4096 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Ffdhe4096),
                KasDpGeneration.Ffdhe6144 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Ffdhe6144),
                KasDpGeneration.Ffdhe8192 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Ffdhe8192),

                KasDpGeneration.Modp2048 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Modp2048),
                KasDpGeneration.Modp3072 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Modp3072),
                KasDpGeneration.Modp4096 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Modp4096),
                KasDpGeneration.Modp6144 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Modp6144),
                KasDpGeneration.Modp8192 => await GenerateSafePrimeDomainParametersAsync(SafePrime.Modp8192),

                KasDpGeneration.Fb => await GenerateDomainParametersDsaAsync(2048, 224),
                KasDpGeneration.Fc => await GenerateDomainParametersDsaAsync(2048, 256),

                _ => throw new ArgumentException(
                    $"Invalid {nameof(domainParameterGenerationMode)} {domainParameterGenerationMode} for this group generator.")
            };
        }

        private Task<FfcDomainParameters> GenerateSafePrimeDomainParametersAsync(SafePrime safePrime)
        {
            return Task.FromResult(SafePrimesFactory.GetDomainParameters(safePrime));
        }

        private async Task<FfcDomainParameters> GenerateDomainParametersDsaAsync(int l, int n)
        {
            var task = _oracle.GetDsaDomainParametersAsync(new DsaDomainParametersParameters()
            {
                L = l,
                N = n,
                GGenMode = GeneratorGenMode.Unverifiable,
                PQGenMode = PrimeGenMode.Probable,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
            });

            var result = await task;

            return new FfcDomainParameters(result.P, result.Q, result.G);
        }

        private async Task<FfcKeyPair> GetKey(FfcDomainParameters domainParameters)
        {
            var task = _oracle.GetDsaKeyAsync(new DsaKeyParameters()
            {
                DomainParameters = domainParameters
            });
            var result = await task;

            return result.Key;
        }
    }
}
