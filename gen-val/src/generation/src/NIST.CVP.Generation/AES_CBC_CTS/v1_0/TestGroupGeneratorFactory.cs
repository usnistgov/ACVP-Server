using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> list =
                new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorKnownAnswerTestsPartialBlock(),
                    new TestGroupGeneratorMultiBlockMessagePartialBlock(),
                };

            // Original CBC known answer tests
            if (parameters.PayloadLen.IsWithinDomain(128))
            {
                list.Add(new TestGroupGeneratorKnownAnswerTestsSingleBlock());
                list.Add(new TestGroupGeneratorMultiBlockMessageFullBlock());
            }

            return list;
        }
    }
}
