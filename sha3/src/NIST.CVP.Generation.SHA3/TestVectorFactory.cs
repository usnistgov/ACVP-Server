using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly string[] _testTypes = {"aft", "mct", "vot"};
       
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            return new TestVectorSet {TestGroups = BuildTestGroups(parameters), Algorithm = parameters.Algorithm, IsSample = parameters.IsSample};
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var testType in _testTypes)
                {
                    var testGroup = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digestSize,
                        IncludeNull = parameters.IncludeNull,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        MinOutputLength = parameters.MinOutputLength,
                        MaxOutputLength = parameters.MaxOutputLength,
                        TestType = testType
                    };

                    // We cannot have SHA3 + VOT, so we just don't add that one
                    if (!(testGroup.Function.ToLower() == "sha3" && testGroup.TestType.ToLower() == "vot"))
                    {
                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
