using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorKnownAnswerTests(),
                    new TestGroupGeneratorMultiBlockMessage(),
                    new TestGroupGeneratorMonteCarlo()
                };

            return list;
        }
    }
}
