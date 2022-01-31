using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        private readonly Dictionary<PrimeGenFips186_4Modes, string[]> _aftProperties = new Dictionary<PrimeGenFips186_4Modes, string[]>
        {
            {
                PrimeGenFips186_4Modes.B32,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E)
                }
            },
            {
                PrimeGenFips186_4Modes.B33,
                new []{""}
            },
            {
                PrimeGenFips186_4Modes.B34,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E),
                    nameof(TestCase.Bitlens),
                }
            },
            {
                PrimeGenFips186_4Modes.B35,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E),
                    nameof(TestCase.Bitlens),
                    nameof(TestCase.XP),
                    nameof(TestCase.XQ)
                }
            },
            {
                PrimeGenFips186_4Modes.B36,
                new []
                {
                    nameof(TestCase.E),
                    nameof(TestCase.Bitlens),
                    nameof(TestCase.XP),
                    nameof(TestCase.XQ),
                    nameof(TestCase.XP1),
                    nameof(TestCase.XQ1),
                    nameof(TestCase.XP2),
                    nameof(TestCase.XQ2)
                }
            }
        };

        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize =
                instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
            };

            var standardPrivateKey = new[]
            {
                nameof(TestCase.D)
            };

            var crtPrivateKey = new[]
            {
                nameof(TestCase.Dmp1),
                nameof(TestCase.Dmq1),
                nameof(TestCase.Iqmp)
            };

            var keyProperties = new[]
            {
                nameof(TestCase.P),
                nameof(TestCase.Q),
                nameof(TestCase.N),
                nameof(TestCase.E)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                // Kats only have one property
                if (testGroup.TestType.Equals("kat", StringComparison.OrdinalIgnoreCase))
                {
                    if (jsonProperty.UnderlyingName.Equals(nameof(TestCase.TestPassed), StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                else
                {
                    if (testGroup.KeyFormat == PrivateKeyModes.Standard)
                    {
                        if (standardPrivateKey.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (crtPrivateKey.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    // Always include these key properties
                    if (keyProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    // Include properties based on PrimeGenMode
                    if (_aftProperties[testGroup.PrimeGenMode].Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            };

            return jsonProperty.ShouldSerialize;
        }
    }
}
