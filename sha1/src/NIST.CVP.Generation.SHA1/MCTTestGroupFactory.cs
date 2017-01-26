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
            // We don't actually need anything from the parameters... 
            // None of the testGroup values will change

            var testGroups = new List<TestGroup>();

            var testGroup = new TestGroup()
            {
                BitOriented = false,
                IncludeNull = false,
                DigestLength = 160,
                MessageLength = 160,
                TestType = _MCT_TEST_TYPE_LABEL
            };
            
            testGroups.Add(testGroup);

            return testGroups;
        }
    }
}
