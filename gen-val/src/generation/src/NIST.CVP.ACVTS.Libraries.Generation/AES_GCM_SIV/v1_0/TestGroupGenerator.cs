using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM_SIV.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var ptLengths = new List<int>();
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetDomainMinMaxAsEnumerable());
            // Get block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PayloadLen
                    .GetRandomValues(g => g % 128 == 0 && !ptLengths.Contains(g), 2));
            // get non block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PayloadLen
                    .GetRandomValues(g => g % 8 == 0 && g % 128 != 0 && !ptLengths.Contains(g), 2));

            var aadLengths = new List<int>();
            aadLengths.AddRangeIfNotNullOrEmpty(parameters.AadLen.GetDomainMinMaxAsEnumerable());
            // Get random values
            aadLengths.AddRangeIfNotNullOrEmpty(
                parameters.AadLen
                    .GetRandomValues(g => g % 8 == 0 && !aadLengths.Contains(g), 2));

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var ptLength in ptLengths)
                    {
                        foreach (var aadLength in aadLengths)
                        {
                            var testGroup = new TestGroup
                            {
                                Function = function,
                                KeyLength = keyLength,
                                PayloadLength = ptLength,
                                AadLength = aadLength
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
