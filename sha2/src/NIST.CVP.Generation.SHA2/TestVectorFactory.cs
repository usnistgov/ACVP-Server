using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IMonteCarloTestGroupFactory<Parameters, TestGroup> _iMCTTestGroupFactory;
        private readonly string[] _testTypes = new string[] {"shortmessage", "longmessage"};

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
                        Function = GetMode(size),
                        DigestSize = GetSize(size),
                        TestType = testType,
                        IncludeNull = parameters.IncludeNull,
                        BitOriented = parameters.BitOriented
                    };
                    testGroups.Add(testGroup);
                }
            }
            
            return testGroups;
        }

        private ModeValues GetMode(string digSize)
        {
            if (digSize.Contains("160"))
            {
                return ModeValues.SHA1;
            }
            else
            {
                return ModeValues.SHA2;
            }
        }

        private DigestSizes GetSize(string digSize)
        {
            if (digSize.Contains("512t256"))
            {
                return DigestSizes.d512t256;
            }
            else if (digSize.Contains("512t224"))
            {
                return DigestSizes.d512t224;
            }
            else if (digSize.Contains("512"))
            {
                return DigestSizes.d512;
            }
            else if (digSize.Contains("384"))
            {
                return DigestSizes.d384;
            }
            else if (digSize.Contains("256"))
            {
                return DigestSizes.d256;
            }
            else if (digSize.Contains("224"))
            {
                return DigestSizes.d224;
            }
            else
            {
                return DigestSizes.d160;
            }
        }
    }
}
