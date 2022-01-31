using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            // Do only RFC3686 group for RFC3686 testing
            if (parameters.Conformances != null && parameters.Conformances.Contains("RFC3686"))
            {
                return new[]
                {
                    new TestGroupGeneratorRfc(),
                };
            }

            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorSingleBlockMessage(),
                new TestGroupGeneratorPartialBlockMessage(),
                new TestGroupGeneratorCounter(),
                new TestGroupGeneratorKnownAnswerTest(),
            };

            return list;
        }
    }
}
