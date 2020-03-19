using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC.v1_0
{
    public class TestGroupGeneratorKnownAnswerTests : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        private readonly string[] _katTests = new string[]
        {
            "GFSBox",
            "KeySBox",
            "VarTxt",
            "VarKey"
        };

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var katTest in _katTests)
                    {
                        var testGroup = new TestGroup()
                        {
                            Function = direction,
                            KeyLength = keyLength,
                            TestType = TEST_TYPE,
                            InternalTestType = katTest
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
