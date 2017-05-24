using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class MCTTestGroupFactory : IMonteCarloTestGroupFactory<Parameters, TestGroup>
    {
        public IEnumerable<TestGroup> BuildMCTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digSize),
                    TestType = "MCT"
                };

                testGroups.Add(testGroup);
            }
            
            return testGroups;
        }
    }
}

