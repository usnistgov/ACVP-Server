using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();

            foreach (var hashAlg in parameters.HashAlg)
            {
                var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);

                var nInitValues = GetMinMaxOtherValueForDomain(parameters.InitiatorNonceLength.GetDeepCopy()).Shuffle();
                var nRespValues = GetMinMaxOtherValueForDomain(parameters.ResponderNonceLength.GetDeepCopy()).Shuffle();
                var dhValues = GetMinMaxOtherValueForDomain(parameters.DiffieHellmanSharedSecretLength.GetDeepCopy()).Shuffle();
                var dkmValues = GetMinMaxOtherValueForDomain(parameters.DerivedKeyingMaterialLength.GetDeepCopy()).Shuffle();

                // Safe assumption that these will all have exactly 3 values
                for (var i = 0; i < 3; i++)
                {
                    // If the dkmValue is too low, make sure it is at least equal to the hash function
                    if (dkmValues[i] < hashFunction.OutputLen)
                    {
                        dkmValues[i] = hashFunction.OutputLen;
                    }

                    var testGroup = new TestGroup
                    {
                        HashAlg = hashFunction,
                        NInitLength = nInitValues[i],
                        NRespLength = nRespValues[i],
                        GirLength = dhValues[i],
                        DerivedKeyingMaterialLength = dkmValues[i]
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }

        private List<int> GetMinMaxOtherValueForDomain(MathDomain domain)
        {
            var min = domain.GetDomainMinMax().Minimum;
            var max = domain.GetDomainMinMax().Maximum;

            var values = new List<int>
            {
                min,
                max
            };

            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var otherValue = domain.GetValues(w => w != min && w != max, 1, true);
            if (otherValue.Any())
            {
                values.Add(otherValue.First());
            }
            else
            {
                values.Add(min);
            }

            // Always will have 3 values
            return values;
        }
    }
}
