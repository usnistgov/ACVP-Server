using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly string[] _testTypes = {"aft", "mct"};

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            return new TestVectorSet {TestGroups = groups, Algorithm = "SHA", IsSample = parameters.IsSample};
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Functions)
            {
                foreach (var digestSize in function.DigestSizes)
                {
                    foreach (var testType in _testTypes)
                    {
                        var testGroup = new TestGroup
                        {
                            Function = SHAEnumHelpers.StringToMode(function.Mode),
                            DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                            TestType = testType,
                            IncludeNull = parameters.IncludeNull,
                            BitOriented = parameters.BitOriented
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }
            
            return testGroups;
        }
    }
}
