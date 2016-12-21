using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_GCM;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "AES-ECB", IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var ptLength in parameters.PtLen)
                    {
                        var testGroup = new TestGroup
                        {
                            Function = function,
                            PTLength = ptLength,
                            KeyLength = keyLength,
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }
            return testGroups;
        }
    }
}
