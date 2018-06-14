using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
