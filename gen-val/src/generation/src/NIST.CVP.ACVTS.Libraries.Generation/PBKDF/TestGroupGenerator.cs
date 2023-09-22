using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private int _baseTestCasesPerGroup = 50;
        private int _highIterationsToPullPerGroup = 5;

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var hmacAlgCount = capability.HashAlg.Length;

                var iterationCounts = GetIterationCounts(capability.IterationCount, parameters.IsSample);
                var keyLengths = GetKeyLengthsFromDomain(capability.KeyLength);
                var passwordLengths = GetValuesFromDomain(capability.PasswordLength);
                var saltLengths = GetValuesFromDomain(capability.SaltLength);

                var numberOfTestCasesToGeneratePerGroup = _baseTestCasesPerGroup;

                if (parameters.IsSample)
                {
                    numberOfTestCasesToGeneratePerGroup = 15;
                }

                // if there are only 1 or 2 valid values within the domain, and that max is >= 100,000.
                // or if all of the possible iterations amounts are greater than 100,000
                if ((iterationCounts.iterationCountList.GroupBy(g => g).Count() <= 2 && iterationCounts.iterationCountList.Max() >= 100000)
                    || iterationCounts.iterationCountList.All(v => v >= 100000))
                {
                    numberOfTestCasesToGeneratePerGroup = 5;
                }
                else
                {
                    // Further limit the test cases to generate per group based on the number of registered hash algorithms,
                    // as we don't get many additional assurances by testing exhaustively against all the hash functions.
                    numberOfTestCasesToGeneratePerGroup =
                        System.Math.Max(numberOfTestCasesToGeneratePerGroup.CeilingDivide(hmacAlgCount), 10);
                }

                groups.AddRange(capability.HashAlg.Select(hashAlg => new TestGroup
                {
                    IterationCount = iterationCounts.iterationQueue,
                    KeyLength = keyLengths,
                    PasswordLength = passwordLengths,
                    SaltLength = saltLengths,
                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                    TestCasesForGroup = numberOfTestCasesToGeneratePerGroup
                }));
            }

            return Task.FromResult(groups);
        }

        private (ShuffleQueue<int> iterationQueue, List<int> iterationCountList) GetIterationCounts(MathDomain domain, bool isSample)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> { minMax.Minimum, minMax.Maximum };
            var smallValuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v < 100000,
                _baseTestCasesPerGroup - _highIterationsToPullPerGroup);
            valuesSelected.AddRange(smallValuesPulled);

            if (!isSample)
            {
                var largeValuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v >= 100000,
                    _highIterationsToPullPerGroup);

                valuesSelected.AddRange(largeValuesPulled);
            }

            return (new ShuffleQueue<int>(valuesSelected), valuesSelected);
        }

        private ShuffleQueue<int> GetKeyLengthsFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> { minMax.Minimum, minMax.Maximum };
            var valuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v <= 2048,
                _baseTestCasesPerGroup - _highIterationsToPullPerGroup);
            valuesSelected.AddRange(valuesPulled);

            var largeValuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum && v > 2048,
                _highIterationsToPullPerGroup);
            valuesSelected.AddRange(largeValuesPulled);

            return new ShuffleQueue<int>(valuesSelected);
        }

        private ShuffleQueue<int> GetValuesFromDomain(MathDomain domain)
        {
            var minMax = domain.GetDomainMinMax();

            var valuesSelected = new List<int> { minMax.Minimum, minMax.Maximum };
            var valuesPulled = domain.GetRandomValues(v => v != minMax.Minimum && v != minMax.Maximum,
                _baseTestCasesPerGroup);
            valuesSelected.AddRange(valuesPulled);

            return new ShuffleQueue<int>(valuesSelected);
        }
    }
}
