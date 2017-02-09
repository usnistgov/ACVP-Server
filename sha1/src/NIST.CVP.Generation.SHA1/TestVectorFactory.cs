using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
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
            if(mctGroups != null && mctGroups.Count() != 0)
            {
                groups.AddRange(mctGroups);
            }

            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "SHA1", IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var msgLen in parameters.MessageLen)
            {
                foreach (var digLen in parameters.DigestLen)
                {
                    var testGroup = new TestGroup
                    {
                        BitOriented = parameters.BitOriented,
                        IncludeNull = parameters.IncludeNull,
                        MessageLength = msgLen,
                        DigestLength = digLen,
                        TestType = "MMT"
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
