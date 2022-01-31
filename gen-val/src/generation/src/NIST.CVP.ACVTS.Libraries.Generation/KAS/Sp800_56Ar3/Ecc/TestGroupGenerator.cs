using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ecc
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
            var curves = groups.Select(s => s.Curve).Distinct();
            var curveDict = new Dictionary<Curve, Task<EccDomainParameters>>();

            foreach (var mode in curves)
            {
                curveDict.Add(mode, GenerateDomainParametersAsync(mode));
            }

            await Task.WhenAll(curveDict.Values);

            foreach (var group in groups)
            {
                group.DomainParameters = await curveDict.First(w => w.Key == group.Curve).Value;
            }
        }

        protected override async Task GenerateKeysPerDomainParametersAsync(List<TestGroup> groupList)
        {
            var curves = groupList.Select(s => s.Curve).Distinct();

            var tasks = new Dictionary<Curve, List<Task<EccKeyPair>>>();

            foreach (var curve in curves)
            {
                var list = new List<Task<EccKeyPair>>();
                for (var i = 0; i < 100; i++)
                {
                    list.Add(GetKey(curve));
                }

                tasks.Add(curve, list);
            }

            await Task.WhenAll(tasks.SelectMany(s => s.Value));

            var shuffleQueues = new Dictionary<Curve, ShuffleQueue<EccKeyPair>>();
            foreach (var kvp in tasks)
            {
                List<EccKeyPair> keys = new List<EccKeyPair>();
                foreach (var task in kvp.Value)
                {
                    keys.Add(await task);
                }
                shuffleQueues.Add(kvp.Key, new ShuffleQueue<EccKeyPair>(keys));
            }

            foreach (var group in groupList)
            {
                group.ShuffleKeys = shuffleQueues.First(f => f.Key == group.Curve).Value;
            }
        }

        private async Task<EccKeyPair> GetKey(Curve curve)
        {
            var task = _oracle.GetEcdsaKeyAsync(new EcdsaKeyParameters()
            {
                Curve = curve,
                Disposition = EcdsaKeyDisposition.None
            });
            var result = await task;

            return result.Key;
        }

        private async Task<EccDomainParameters> GenerateDomainParametersAsync(Curve curve)
        {
            var task = _oracle.GetEcdsaDomainParameterAsync(new EcdsaCurveParameters() { Curve = curve });
            var result = await task;

            return result.EccDomainParameters;
        }
    }
}
