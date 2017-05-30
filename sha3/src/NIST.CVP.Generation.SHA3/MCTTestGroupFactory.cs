using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
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
                    Function = parameters.Algorithm,
                    DigestSize = digSize,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    MinOutputLength = parameters.MinOutputLength,
                    MaxOutputLength = parameters.MaxOutputLength,
                    TestType = "MCT"
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
