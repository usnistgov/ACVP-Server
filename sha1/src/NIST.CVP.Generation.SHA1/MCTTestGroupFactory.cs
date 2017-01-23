using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class MCTTestGroupFactory : IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public IEnumerable<TestGroup> BuildMCTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var testGroup = new TestGroup()
            {
                DigestLength = parameters.DigestLen[0],
                MessageLength = parameters.DigestLen[0] * 3,
                TestType = _MCT_TEST_TYPE_LABEL
            };
            
            testGroups.Add(testGroup);

            return testGroups;
        }
    }
}
