﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.RSA_KeyGen.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        private readonly Dictionary<PrimeGenModes, string[]> _aftProperties = new Dictionary<PrimeGenModes, string[]>
        {
            {
                PrimeGenModes.B32,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E)
                }
            },
            {
                PrimeGenModes.B33,
                new []{""}
            },
            {
                PrimeGenModes.B34,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E),
                    nameof(TestCase.Bitlens),
                }
            },
            {
                PrimeGenModes.B35,
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
                PrimeGenModes.B36,
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