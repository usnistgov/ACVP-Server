using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, FfcDomainParameters, FfcKeyPair>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        protected override KasDpGeneration[] GetFilteredDpGeneration(KasDpGeneration[] dpGeneration)
        {
            var parameterSets = new List<KasDpGeneration>()
            {
                KasDpGeneration.Fb,
                KasDpGeneration.Fc,
            }.Shuffle();
            var safePrimesModp = new List<KasDpGeneration>()
            {
                KasDpGeneration.Modp2048,
                KasDpGeneration.Modp3072,
                KasDpGeneration.Modp4096,
                KasDpGeneration.Modp6144,
                KasDpGeneration.Modp8192
            }.Shuffle();
            var safePrimesFfdhe = new List<KasDpGeneration>()
            {
                KasDpGeneration.Ffdhe2048,
                KasDpGeneration.Ffdhe3072,
                KasDpGeneration.Ffdhe4096,
                KasDpGeneration.Ffdhe6144,
                KasDpGeneration.Ffdhe8192
            }.Shuffle();

            var selectedGroups = new List<KasDpGeneration>();
            var maxToPullPerGroup = 2;
            
            selectedGroups.AddRange(parameterSets.Intersect(dpGeneration).Take(maxToPullPerGroup));
            selectedGroups.AddRange(safePrimesModp.Intersect(dpGeneration).Take(maxToPullPerGroup));
            selectedGroups.AddRange(safePrimesFfdhe.Intersect(dpGeneration).Take(maxToPullPerGroup));

            return selectedGroups.ToArray();
        }

        protected override async Task GenerateDomainParametersAsync(List<TestGroup> groups)
        {
            var map = new Dictionary<TestGroup, Task<FfcDomainParameters>>();
            foreach (var group in groups)
            {
                map.Add(group, GenerateDomainParametersAsync(group));
            }

            foreach (var (group, value) in map)
            {
                group.DomainParameters = await value;
            }
        }
        
        private async Task<FfcDomainParameters> GenerateDomainParametersAsync(TestGroup @group)
        {
            return group.DomainParameterGenerationMode switch
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
                    $"Invalid {nameof(group.DomainParameterGenerationMode)} {group.DomainParameterGenerationMode} for this group generator.")
            };
        }

        private async Task<FfcDomainParameters> GenerateSafePrimeDomainParametersAsync(SafePrime safePrime)
        {
            var task = _oracle.GetSafePrimeGroupsDomainParameterAsync(new SafePrimeParameters() { SafePrime = safePrime});
            var result = await task;
            
            return new FfcDomainParameters(result.DomainParameters.P, result.DomainParameters.Q, result.DomainParameters.G);
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
    }
}