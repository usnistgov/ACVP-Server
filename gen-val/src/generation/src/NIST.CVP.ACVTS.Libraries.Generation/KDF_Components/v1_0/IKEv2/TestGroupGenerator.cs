using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2
{

    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                    var dkmChildValues = GetMinMaxOtherValueForDomain(capability.DerivedKeyingMaterialChildLength.GetDeepCopy()).Shuffle();

                    // Safe assumption that these will all have exactly 3 values
                    for (var i = 0; i < 3; i++)
                    {

                        // If the dkmValue is too low, make sure it is at least equal to the hash function
                        if (dkmValues[i] < hashFunction.OutputLen)
                        {
                            dkmValues[i] = hashFunction.OutputLen;
                        }
                        // Assume we should do the same check for dkmChildValue
                        if (dkmChildValues[i] < hashFunction.OutputLen)
                        {
                            dkmChildValues[i] = hashFunction.OutputLen;
                        }

                        TestGroup testGroup;
                        if (!capability.DkmChildLenEqDkmLen)
                        {
                            testGroup = new TestGroup
                            {
                                HashAlg = hashFunction,
                                NInitLength = nInitValues[i],
                                NRespLength = nRespValues[i],
                                GirLength = dhValues[i],
                                DerivedKeyingMaterialLength = dkmValues[i],
                                DerivedKeyingMaterialChildLength = dkmChildValues[i],
                            };
                        }
                        else // If capability.DkmChildLenEqDkmLen, i.e., no derivedKeyingMaterialChildLength was provided
                             // in the registration, then mimic the IKEv2 behavior prior to adding the derivedKeyingMaterialChildLength 
                             // property, i.e., have derivedKeyingMaterialLength and derivedKeyingMaterialChildLength values match
                             // each other in each test group.
                        {
                            testGroup = new TestGroup
                            {
                                HashAlg = hashFunction,
                                NInitLength = nInitValues[i],
                                NRespLength = nRespValues[i],
                                GirLength = dhValues[i],
                                DerivedKeyingMaterialLength = dkmValues[i],
                                DerivedKeyingMaterialChildLength = dkmValues[i],
                            };                            
                        }

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups.ToList());
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
