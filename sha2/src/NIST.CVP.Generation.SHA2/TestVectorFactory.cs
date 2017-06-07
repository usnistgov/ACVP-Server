using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IMonteCarloTestGroupFactory<Parameters, TestGroup> _iMCTTestGroupFactory;

        public TestVectorFactory(IMonteCarloTestGroupFactory<Parameters, TestGroup> iMCTTestGroupFactory)
        {
            _iMCTTestGroupFactory = iMCTTestGroupFactory;
        }

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);

            var mctGroups = _iMCTTestGroupFactory.BuildMCTTestGroups(parameters);
            if (mctGroups != null && mctGroups.Count() != 0)
            {
                groups.AddRange(mctGroups);
            }

            return new TestVectorSet {TestGroups = groups, Algorithm = parameters.Algorithm, IsSample = parameters.IsSample};
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                    TestType = "AFT",
                    IncludeNull = parameters.IncludeNull,
                    BitOriented = parameters.BitOriented
                    //IncludeNull = StringToBoolean(parameters.IncludeNull),
                    //BitOriented = StringToBoolean(parameters.BitOriented)
                };
                testGroups.Add(testGroup);
            }
            
            return testGroups;
        }

        private bool StringToBoolean(string str)
        {
            if (str.ToLower() == "yes")
            {
                return true;
            }
            else if (str.ToLower() == "no")
            {
                return false;
            }

            throw new Exception("Boolean string is not yes or no");
        }
    }
}
