using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            return new TestVectorSet
            {
                TestGroups = groups,
                Algorithm = parameters.Algorithm,
                IsSample = parameters.IsSample
            };
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();

            return testGroups;
        }
    }
}
