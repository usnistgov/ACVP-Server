using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>> list =
                new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorKnownAnswerTestsPartialBlock(),
                    new TestGroupGeneratorMonteCarlo()
                };

            if (parameters.PayloadLen.IsWithinDomain(128))
            {
                list.Add(new TestGroupGeneratorKnownAnswerTestsSingleBlock());
                list.Add(new TestGroupGeneratorMultiBlockMessageFullBlock());
            }

            return list;
        }
    }
}
