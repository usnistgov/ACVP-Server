using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, FfcDomainParameters>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        protected override Task<FfcDomainParameters> GenerateDomainParametersAsync(TestGroup @group)
        {
            return group.DomainParameterGenerationMode switch
            {
                KasDpGeneration.Ffdhe2048 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Ffdhe2048),
                KasDpGeneration.Ffdhe3072 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Ffdhe3072),
                KasDpGeneration.Ffdhe4096 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Ffdhe4096),
                KasDpGeneration.Ffdhe6144 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Ffdhe6144),
                KasDpGeneration.Ffdhe8192 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Ffdhe8192),
                
                KasDpGeneration.Modp2048 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Modp2048),
                KasDpGeneration.Modp3072 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Modp3072),
                KasDpGeneration.Modp4096 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Modp4096),
                KasDpGeneration.Modp6144 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Modp6144),
                KasDpGeneration.Modp8192 => _oracle.GetSafePrimeGroupsDomainParameterAsync(SafePrime.Modp8192),

                KasDpGeneration.Fb => GenerateDomainParametersDsaAsync(2084, 224),
                KasDpGeneration.Fc => GenerateDomainParametersDsaAsync(2084, 256),
                
                _ => throw new ArgumentException(
                    $"Invalid {nameof(group.DomainParameterGenerationMode)} {group.DomainParameterGenerationMode} for this group generator.")
            };
        }

        private async Task<FfcDomainParameters> GenerateDomainParametersDsaAsync(int l, int n)
        {
            var task = _oracle.GetDsaDomainParametersAsync(new DsaDomainParametersParameters()
            {
                L = l,
                N = n,
                GGenMode = GeneratorGenMode.Unverifiable,
                PQGenMode = PrimeGenMode.Probable
            });

            var result = await task;
            
            return new FfcDomainParameters(result.P, result.Q, result.G);
        }
    }
}