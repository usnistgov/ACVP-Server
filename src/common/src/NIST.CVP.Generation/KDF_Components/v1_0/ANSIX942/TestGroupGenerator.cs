using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = new HashSet<TestGroup>();
            foreach (var mode in parameters.KdfType)
            {
                var kdfMode = EnumHelpers.GetEnumFromEnumDescription<AnsiX942Types>(mode);

                foreach (var hashAlg in parameters.HashAlg)
                {
                    var hash = ShaAttributes.GetHashFunctionFromName(hashAlg);

                    var keyLen = GetMinMaxOtherValueForDomain(parameters.KeyLen.GetDeepCopy());
                    var otherInfoLen = GetMinMaxOtherValueForDomain(parameters.OtherInfoLen.GetDeepCopy());
                    var zzLen = GetMinMaxOtherValueForDomain(parameters.ZzLen.GetDeepCopy());
                    
                    for (var i = 0; i < 3; i++)
                    {
                        var group = new TestGroup
                        {
                            HashAlg = hash,
                            KdfType = kdfMode,
                            KeyLen = keyLen[i],
                            OtherInfoLen = otherInfoLen[i],
                            ZzLen = zzLen[i]
                        };

                        groups.Add(group);
                    }
                }
            }

            return groups;
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
