using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
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
                        MessageLength = msgLen,
                        DigestLength = digLen
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
