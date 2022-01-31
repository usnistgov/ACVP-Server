using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var authMode = EnumHelpers.GetEnumFromEnumDescription<AuthenticationMethods>(capability.AuthenticationMethod);

                foreach (var hashAlg in capability.HashAlg)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);

                    var nInitValues = GetMinMaxOtherValueForDomain(capability.InitiatorNonceLength.GetDeepCopy()).Shuffle();
                    var nRespValues = GetMinMaxOtherValueForDomain(capability.ResponderNonceLength.GetDeepCopy()).Shuffle();
                    var dhValues = GetMinMaxOtherValueForDomain(capability.DiffieHellmanSharedSecretLength.GetDeepCopy()).Shuffle();

                    // Safe assumption that these will all have exactly 3 values
                    for (var i = 0; i < 3; i++)
                    {
                        List<int> pskValues;
                        if (authMode == AuthenticationMethods.Psk)
                        {
                            pskValues = GetMinMaxOtherValueForDomain(capability.PreSharedKeyLength.GetDeepCopy()).Shuffle();
                        }
                        else
                        {
                            pskValues = new List<int> { 0, 0, 0 };
                        }

                        var testGroup = new TestGroup
                        {
                            AuthenticationMethod = authMode,
                            HashAlg = hashFunction,
                            NInitLength = nInitValues[i],
                            NRespLength = nRespValues[i],
                            GxyLength = dhValues[i],
                            PreSharedKeyLength = pskValues[i]
                        };

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
