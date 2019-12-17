using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, EccDomainParameters>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        protected override Task<EccDomainParameters> GenerateDomainParametersAsync(TestGroup @group)
        {
            return group.DomainParameterGenerationMode switch
            {
                KasDpGeneration.B163 => _oracle.GetEcdsaDomainParameterAsync(Curve.B163),
                KasDpGeneration.B233 => _oracle.GetEcdsaDomainParameterAsync(Curve.B233),
                KasDpGeneration.B283 => _oracle.GetEcdsaDomainParameterAsync(Curve.B283),
                KasDpGeneration.B409 => _oracle.GetEcdsaDomainParameterAsync(Curve.B409),
                KasDpGeneration.B571 => _oracle.GetEcdsaDomainParameterAsync(Curve.B571),
                KasDpGeneration.K163 => _oracle.GetEcdsaDomainParameterAsync(Curve.K163),
                KasDpGeneration.K233 => _oracle.GetEcdsaDomainParameterAsync(Curve.K233),
                KasDpGeneration.K283 => _oracle.GetEcdsaDomainParameterAsync(Curve.K283),
                KasDpGeneration.K409 => _oracle.GetEcdsaDomainParameterAsync(Curve.K409),
                KasDpGeneration.K571 => _oracle.GetEcdsaDomainParameterAsync(Curve.K571),
                KasDpGeneration.P192 => _oracle.GetEcdsaDomainParameterAsync(Curve.P192),
                KasDpGeneration.P224 => _oracle.GetEcdsaDomainParameterAsync(Curve.P224),
                KasDpGeneration.P256 => _oracle.GetEcdsaDomainParameterAsync(Curve.P256),
                KasDpGeneration.P384 => _oracle.GetEcdsaDomainParameterAsync(Curve.P384),
                KasDpGeneration.P521 => _oracle.GetEcdsaDomainParameterAsync(Curve.P521),
                _ => throw new ArgumentException(
                    $"Invalid {nameof(group.DomainParameterGenerationMode)} {group.DomainParameterGenerationMode} for this group generator.")
            };
        }
    }
}