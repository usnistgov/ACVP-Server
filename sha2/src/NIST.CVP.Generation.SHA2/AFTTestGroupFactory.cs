using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class AFTTestGroupFactory : IAlgorithmFunctionalTestGroupFactory<Parameters, TestGroup>
    {
        public IEnumerable<TestGroup> BuildAFTTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                    TestType = "AFT",
                    IncludeNull = parameters.IncludeNull,
                    BitOriented = parameters.BitOriented
                };
                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
