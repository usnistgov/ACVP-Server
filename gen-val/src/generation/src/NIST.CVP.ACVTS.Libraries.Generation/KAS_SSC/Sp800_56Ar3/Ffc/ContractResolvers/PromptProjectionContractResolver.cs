using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ffc.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includePropertiesAllScenarios = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.DomainParameterGenerationMode),
                nameof(TestGroup.Scheme),
                nameof(TestGroup.KasRole),
                nameof(TestGroup.Tests),
            };

            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            // Return the hashFunctionZ only when being used.
            if (jsonProperty.UnderlyingName.Equals(nameof(TestGroup.HashFunctionZ), StringComparison.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.HashFunctionZ != HashFunctions.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var includeDomainParameters = new[]
            {
                nameof(TestGroup.P),
                nameof(TestGroup.Q),
                nameof(TestGroup.G),
            };

            // We need to serialize the PQG when using "FB" or "FC" (safe prime group is none).
            if (includeDomainParameters.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.SafePrime == SafePrime.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, from both parties
            // AFT tests - include, public keys from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),

                nameof(TestCase.EphemeralPublicKeyServer),
                nameof(TestCase.StaticPublicKeyServer),
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.EphemeralPublicKeyIut),
                nameof(TestCase.StaticPublicKeyIut),
                nameof(TestCase.EphemeralPrivateKeyIut),
                nameof(TestCase.StaticPrivateKeyIut),
            };

            if (includePropertiesValScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            // Include Z when no hash function provided
            if (jsonProperty.UnderlyingName.Equals(nameof(TestCase.Z), StringComparison.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase) && testGroup.HashFunctionZ == HashFunctions.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            // Include hashZ when hash function provided
            if (jsonProperty.UnderlyingName.Equals(nameof(TestCase.HashZ), StringComparison.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase) && testGroup.HashFunctionZ != HashFunctions.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
            #endregion Conditional Test Case properties
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type) ||
                typeof(TestGroupBase<TestGroup, TestCase, FfcKeyPair>).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type) ||
                typeof(TestCaseBase<TestGroup, TestCase, FfcKeyPair>).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
    }
}
