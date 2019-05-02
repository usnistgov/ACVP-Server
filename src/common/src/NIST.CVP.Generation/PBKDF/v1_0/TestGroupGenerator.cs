using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.PBKDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();

            // Pull 5 values from each Domain including min/max
            var potentialKeyLen = GetValuesFromDomain(parameters.KeyLength);
            var potentialIterationCount = GetValuesFromDomain(parameters.IterationCount);
            var potentialSaltLen = GetValuesFromDomain(parameters.SaltLength);
            var potentialPasswordLen = GetValuesFromDomain(parameters.PasswordLength);

            for (var i = 0; i < 5; i++)
            {
                testGroups.Add(new TestGroup
                {
                    IterationCount = potentialIterationCount[i%5],
                    KeyLength = potentialKeyLen[i%5],
                    SaltLength = potentialSaltLen[i%5],
                    PasswordLength = potentialPasswordLen[i%5]
                });
            }
            
            return testGroups;
        }

        private List<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var list = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            // Get random values that are not the min or max
            var values = domain.GetValues(v => v != minMax.Minimum && v != minMax.Maximum, 3, true);

            // Combine min/max and random values
            return list.Union(values).ToList();
        }
    }
}