using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list =
                new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorKnownAnswerTests(),
                    new TestGroupGeneratorMultiBlockMessage(),
                    new TestGroupGeneratorMonteCarlo()
                };

            return list;
        }
    }
}
