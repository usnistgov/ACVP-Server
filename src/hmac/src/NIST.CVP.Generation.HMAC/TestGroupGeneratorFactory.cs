using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.HMAC
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGenerator()
                };

            return list;
        }
    }
}