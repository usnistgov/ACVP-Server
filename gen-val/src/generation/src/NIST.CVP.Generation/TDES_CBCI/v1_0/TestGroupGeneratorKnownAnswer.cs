using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBCI.v1_0
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";
        private readonly string[] _katTests = KatData.GetLabels();

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                {
                    var tg = new TestGroup()
                    {
                        Function = function,
                        KeyingOption = 3,
                        InternalTestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }
            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
