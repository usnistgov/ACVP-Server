using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, EccDomainParameters, EccKeyPair>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        protected override async Task GenerateDomainParametersAsync(List<TestGroup> groups)
        {
            var map = new Dictionary<TestGroup, Task<EccDomainParameters>>();
            foreach (var group in groups)
            {
                map.Add(group, GenerateDomainParametersAsync(group));
            }

            foreach (var pair in map)
            {
                pair.Key.DomainParameters = await pair.Value;
            }
        }
        
        private async Task<EccDomainParameters> GenerateDomainParametersAsync(TestGroup @group)
        {
            var task = _oracle.GetEcdsaDomainParameterAsync(new EcdsaCurveParameters() {Curve = group.Curve});
            var result = await task;
            
            return result.EccDomainParameters;
        }
    }
}