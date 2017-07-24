using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGeneratorAlgorithmFunctionalTest(),
                new TestGroupGeneratorKnownAnswerTests(),
                new TestGroupGeneratorGeneratedDataTest()
            };

            return list;
        }
    }
}
