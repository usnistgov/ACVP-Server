using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public const string ALGORITHM_FUNCTIONAL_TEST = "AFT";
    public const string TREE_EXHAUSTION_TEST = "TET";

    private readonly IOracle _oracle;

    public TestGroupGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }
    
    // Typically this would be broken out into a TestGroupGeneratorAft and a TestGroupGeneratorTet, but they would do identical things
    public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new HashSet<TestGroup>();
        var map = new Dictionary<TestGroup, Task<LmsKeyPairResult>>();

        if (parameters.SpecificCapabilities?.Length > 0)
        {
            foreach (var cap in parameters.SpecificCapabilities)
            {
                var aftTestGroup = new TestGroup { LmsMode = cap.LmsMode, LmOtsMode = cap.LmOtsMode, TestType = ALGORITHM_FUNCTIONAL_TEST };
                
                if (parameters.IsSample)
                {
                    // Generate a tree
                    var param = new LmsKeyPairParameters { LmsMode = cap.LmsMode, LmOtsMode = cap.LmOtsMode };
                    map.Add(aftTestGroup, _oracle.GetLmsKeyCaseAsync(param));
                }
            }
        }
        else
        {
            var mappedOutputLengths = AttributesHelper.GetMappedLmsLmOtsModesToFunctionOutputLength(parameters.Capabilities.LmsModes, parameters.Capabilities.LmOtsModes);

            foreach (var item in mappedOutputLengths)
            {
                var lmsModes = new ShuffleQueue<LmsMode>(item.LmsModes);
                var lmOtsModes = new ShuffleQueue<LmOtsMode>(item.LmOtsModes);

                var maxLen = new[] { lmsModes.OriginalListCount, lmOtsModes.OriginalListCount }.Max();

                for (var i = 0; i < maxLen; i++)
                {
                    var lmsMode = lmsModes.Pop();
                    var lmOtsMode = lmOtsModes.Pop();

                    var aftTestGroup = new TestGroup { LmsMode = lmsMode, LmOtsMode = lmOtsMode, TestType = ALGORITHM_FUNCTIONAL_TEST };

                    if (parameters.IsSample)
                    {
                        // Generate a tree
                        var param = new LmsKeyPairParameters { LmsMode = lmsMode, LmOtsMode = lmOtsMode };
                        map.Add(aftTestGroup, _oracle.GetLmsKeyCaseAsync(param));
                    }
                }
            }
        }
        
        await Task.WhenAll(map.Values);
        foreach (var (group, value) in map)
        {
            var key = value.Result;
            group.KeyPair = key.KeyPair;
            group.PublicKey = new BitString(key.KeyPair.PublicKey.Key);
            testGroups.Add(group);
        }

        return testGroups.ToList();
    }
}
