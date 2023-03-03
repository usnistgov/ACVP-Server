using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();

            if (parameters.SpecificCapabilities?.Length > 0)
            {
                foreach (var cap in parameters.SpecificCapabilities)
                {
                    testGroups.Add(new() { LmsMode = cap.LmsMode, LmOtsMode = cap.LmOtsMode });
                }
            }
            else
            {
                var mappedOutputLengths = AttributesHelper
                    .GetMappedLmsLmOtsModesToFunctionOutputLength(
                        parameters.Capabilities.LmsModes, parameters.Capabilities.LmOtsModes);

                foreach (var item in mappedOutputLengths)
                {
                    var lmsModes = new ShuffleQueue<LmsMode>(item.LmsModes);
                    var lmOtsModes = new ShuffleQueue<LmOtsMode>(item.LmOtsModes);

                    var maxLen = new[] { lmsModes.OriginalListCount, lmOtsModes.OriginalListCount }.Max();

                    for (var i = 0; i < maxLen; i++)
                    {
                        testGroups.Add(new() { LmsMode = lmsModes.Pop(), LmOtsMode = lmOtsModes.Pop() });
                    }
                }
            }

            return Task.FromResult(testGroups.ToList());
        }
    }
}
