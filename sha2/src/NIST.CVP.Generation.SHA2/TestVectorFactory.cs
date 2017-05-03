using System;
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
            return new TestVectorSet {TestGroups = groups, Algorithm = parameters.Algorithm, IsSample = parameters.IsSample};
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
                        Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                        DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                        TestType = testType,
                        IncludeNull = StringToBoolean(parameters.IncludeNull),
                        BitOriented = StringToBoolean(parameters.BitOriented)
                    };
                    testGroups.Add(testGroup);
                }
            }
            
            return testGroups;
        }

        private bool StringToBoolean(string str)
        {
            if (str.ToLower() == "yes")
            {
                return true;
            }else if (str.ToLower() == "no")
            {
                return false;
            }

            throw new Exception("Boolean string is not yes or no");
        }
    }
}
