using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class TestGroupGenerator : TestGroupGeneratorBase<TestGroup, TestCase, EccDomainParameters, EccKeyPair>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        protected override KasDpGeneration[] GetFilteredDpGeneration(KasDpGeneration[] dpGeneration)
        {
            var bCurves = new List<KasDpGeneration>()
            {
                KasDpGeneration.B233,
                KasDpGeneration.B283,
                KasDpGeneration.B409,
                KasDpGeneration.B571
            }.Shuffle();
            var kCurves = new List<KasDpGeneration>()
            {
                KasDpGeneration.K233,
                KasDpGeneration.K283,
                KasDpGeneration.K409,
                KasDpGeneration.K571
            }.Shuffle();
            var pCurves = new List<KasDpGeneration>()
            {
                KasDpGeneration.P224,
                KasDpGeneration.P256,
                KasDpGeneration.P384,
                KasDpGeneration.P521
            }.Shuffle();

            var selectedCurves = new List<KasDpGeneration>();
            var maxToPullPerCurveGroup = 2;
            
            selectedCurves.AddRange(bCurves.Intersect(dpGeneration).Take(maxToPullPerCurveGroup));
            selectedCurves.AddRange(kCurves.Intersect(dpGeneration).Take(maxToPullPerCurveGroup));
            selectedCurves.AddRange(pCurves.Intersect(dpGeneration).Take(maxToPullPerCurveGroup));

            return selectedCurves.ToArray();
        }

        protected override async Task GenerateDomainParametersAsync(List<TestGroup> groups)
        {
            var map = new Dictionary<TestGroup, Task<EccDomainParameters>>();
            foreach (var group in groups)
            {
                map.Add(group, GenerateDomainParametersAsync(group));
            }

            await Task.WhenAll(map.Values);
            
            foreach (var pair in map)
            {
                pair.Key.DomainParameters = pair.Value.Result;
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