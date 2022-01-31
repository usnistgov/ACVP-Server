using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorRfc : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "RFC3686";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // Skip building these groups is the option is disabled
            if (!parameters.PerformCounterTests)
            {
                return Task.FromResult(testGroups);
            }

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {

                    var testGroup = new TestGroup
                    {
                        IvGenMode = parameters.IvGenMode,
                        PayloadLength = parameters.PayloadLen.GetDeepCopy(),
                        Direction = direction,
                        KeyLength = keyLength,
                        IncrementalCounter = true,
                        OverflowCounter = false,
                        InternalTestType = LABEL,
                        RfcTestMode = true
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
