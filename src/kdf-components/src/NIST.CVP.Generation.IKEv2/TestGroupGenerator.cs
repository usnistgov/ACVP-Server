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


            foreach (var capability in parameters.Capabilities)
            {

                foreach (var hashAlg in capability.HashAlg)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);


                    var nInitValues = GetMinMaxOtherValueForDomain(capability.InitiatorNonceLength.GetDeepCopy()).Shuffle();
                    var nRespValues = GetMinMaxOtherValueForDomain(capability.ResponderNonceLength.GetDeepCopy()).Shuffle();
                    var dhValues = GetMinMaxOtherValueForDomain(capability.DiffieHellmanSharedSecretLength.GetDeepCopy()).Shuffle();
                    var dkmValues = GetMinMaxOtherValueForDomain(capability.DerivedKeyingMaterialLength.GetDeepCopy()).Shuffle();


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

            // If we can get some other value, use it, otherwise use the min again
            var otherValue = domain.GetValues(w => w != min && w != max, 1, true);
            var enumerable = otherValue.ToList();
            values.Add(enumerable.Any() ? enumerable.First() : min);

            // Always will have 3 values
            return values;
        }
    }
}
