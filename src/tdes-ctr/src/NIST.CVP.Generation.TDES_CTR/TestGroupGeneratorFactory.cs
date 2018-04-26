using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorSingleBlockMessage(),
                new TestGroupGeneratorPartialBlockMessage(),
                new TestGroupGeneratorCounter(),
                new TestGroupGeneratorKnownAnswerTest()
            };

            return list;
        }
    }
}
