using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ecc.ContractResolvers
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

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, from both parties
            // AFT tests - include, public keys from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),

                nameof(TestCase.EphemeralPublicKeyServerX),
                nameof(TestCase.EphemeralPublicKeyServerY),
                nameof(TestCase.StaticPublicKeyServerX),
                nameof(TestCase.StaticPublicKeyServerY),
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.EphemeralPublicKeyIutX),
                nameof(TestCase.EphemeralPublicKeyIutY),
                nameof(TestCase.StaticPublicKeyIutX),
                nameof(TestCase.StaticPublicKeyIutY),
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
                typeof(TestGroupBase<TestGroup, TestCase, EccKeyPair>).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type) ||
                typeof(TestCaseBase<TestGroup, TestCase, EccKeyPair>).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
    }
}
