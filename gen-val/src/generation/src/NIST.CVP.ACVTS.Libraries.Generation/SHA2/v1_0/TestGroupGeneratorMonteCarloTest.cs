using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class TestGroupGeneratorMonteCarloTest : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = ShaAttributes.StringToMode(parameters.Algorithm),
                    DigestSize = ShaAttributes.StringToDigest(digSize),
                    MessageLength = parameters.MessageLength,
                    TestType = TEST_TYPE
                };
                
                if (parameters.MessageLength != null && !testGroup.MessageLength.IsWithinDomain(testGroup.CommonHashFunction.OutputLen*3))
                {
                    testGroup.MctVersion = MctVersions.Alternate;
                }
                else
                {
                    testGroup.MctVersion = MctVersions.Standard;
                }

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}

