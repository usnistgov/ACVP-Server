using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public const string ALGORITHM_FUNCTIONAL_TEST = "AFT";

    private readonly IOracle _oracle;

    public TestGroupGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }

    // Typically this would be broken out into a TestGroupGeneratorAft and a TestGroupGeneratorTet, but they would do identical things
    public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new HashSet<TestGroup>();
        var map = new Dictionary<TestGroup, Task<XmssKeyPairResult>>();

        foreach (var xmssMode in parameters.Capabilities.XmssModes)
        {
            var aftTestGroup = new TestGroup { XmssMode = xmssMode, TestType = ALGORITHM_FUNCTIONAL_TEST, MessageLength = parameters.MessageLength };

            if (parameters.IsSample)
            {
                // Generate a tree
                var param = new XmssKeyPairParameters { XmssMode = xmssMode };
                map.Add(aftTestGroup, _oracle.GetXmssKeyCaseAsync(param));
            }
            else
            {
                testGroups.Add(aftTestGroup);
            }
        }

        // If we're sample, we need to flush out the trees
        if (parameters.IsSample)
        {
            await Task.WhenAll(map.Values);
            foreach (var (group, value) in map)
            {
                var key = value.Result;
                group.KeyPair = key.KeyPair;
                group.PublicKey = new BitString(key.KeyPair.PublicKey.Key);
                testGroups.Add(group);
            }
        }

        return testGroups.ToList();
    }
}
