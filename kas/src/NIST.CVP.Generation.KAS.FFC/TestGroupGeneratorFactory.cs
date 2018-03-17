using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IPqgProvider _iPqgProvider;
        
        public TestGroupGeneratorFactory(IPqgProvider iPqgProvider)
        {
            _iPqgProvider = iPqgProvider;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list =
                new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGenerator(_iPqgProvider),
                };

            return list;
        }
    }
}
