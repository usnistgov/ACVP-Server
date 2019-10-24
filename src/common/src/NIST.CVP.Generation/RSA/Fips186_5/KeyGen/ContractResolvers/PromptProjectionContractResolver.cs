using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        private readonly Dictionary<PrimeGenModes, string[]> _aftProperties = new Dictionary<PrimeGenModes, string[]>
        {
            {
                PrimeGenModes.RandomProvablePrimes,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E)
                }
            },
            {
                PrimeGenModes.RandomProbablePrimes,
                new []{""}
            },
            {
                PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes,
                new []
                {
                    nameof(TestCase.Seed),
                    nameof(TestCase.E),
                    nameof(TestCase.Bitlens)
                }
            },
            {
                PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes,
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
                PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes,
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
            jsonProperty.ShouldSerialize = instance =>
            {
                GetTestGroupFromTestGroupObject(instance, out var testGroup);

                // Ignore PrimeTest for B.3.2 and B.3.4
                if (testGroup.PrimeGenMode == PrimeGenModes.RandomProvablePrimes ||
                    testGroup.PrimeGenMode == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                {
                    if (jsonProperty.UnderlyingName.Equals(nameof(TestGroup.PrimeTest),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                // Ignore HashAlg for B.3.3, B.3.6
                if (testGroup.PrimeGenMode == PrimeGenModes.RandomProbablePrimes ||
                    testGroup.PrimeGenMode == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes)
                {
                    if (jsonProperty.UnderlyingName.Equals(nameof(TestGroup.HashAlgName),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                // Ignore E if public exponent is random
                if (testGroup.PubExp == PublicExponentModes.Random)
                {
                    if (jsonProperty.UnderlyingName.Equals(nameof(TestGroup.FixedPubExp),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            };

            return jsonProperty.ShouldSerialize;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId)
            };

            var deferredProperties = new[]
            {
                nameof(TestCase.Deferred)
            };

            var b33KatProperties = new[]
            {
                nameof(TestCase.E),
                nameof(TestCase.P),
                nameof(TestCase.Q)
            };

            jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                // Always include
                if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }

                // Include only on deferred (covers GDT groups)
                if (testCase.Deferred)
                {
                    if (deferredProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                else
                {
                    // Include based on group
                    if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        if (_aftProperties[testGroup.PrimeGenMode].Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    if (testGroup.TestType.Equals("kat", StringComparison.OrdinalIgnoreCase))
                    {
                        if (b33KatProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                return false;
            };

            return jsonProperty.ShouldSerialize;
        }
    }
}