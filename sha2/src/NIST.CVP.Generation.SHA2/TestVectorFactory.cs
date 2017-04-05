using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IMonteCarloTestGroupFactory<Parameters, TestGroup> _iMCTTestGroupFactory;
        private readonly string[] _testTypes = {"aft"};

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

            return new TestVectorSet {TestGroups = groups, Algorithm = "SHA", IsSample = parameters.IsSample};
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var size in parameters.DigestSize)
            {
                foreach (var testType in _testTypes)
                {
                    var testGroup = new TestGroup
                    {
                        Function = SHAEnumHelpers.DigestSizeToMode(size),
                        DigestSize = SHAEnumHelpers.StringToDigest(size),
                        TestType = testType,
                        IncludeNull = parameters.IncludeNull,
                        BitOriented = parameters.BitOriented
                    };
                    testGroups.Add(testGroup);
                }
            }
            
            return testGroups;
        }
    }
}
