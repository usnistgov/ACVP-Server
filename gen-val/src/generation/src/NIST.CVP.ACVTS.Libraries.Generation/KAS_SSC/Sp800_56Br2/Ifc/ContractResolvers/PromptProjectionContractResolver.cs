using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includePropertiesAllScenarios = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.KeyGenerationMethod),
                nameof(TestGroup.Modulo),
                nameof(TestGroup.Scheme),
                nameof(TestGroup.KasRole),
                nameof(TestGroup.PublicExponent),
                nameof(TestGroup.Tests),
            };

            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            var includeFixedPublicExponent = new[]
            {
                nameof(TestGroup.PublicExponent)
            };
            var fixedPubExpIncludeWithKeyGens = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_basic,
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor
            };
            if (includeFixedPublicExponent.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);
                        return fixedPubExpIncludeWithKeyGens.Contains(testGroup.KeyGenerationMethod);
                    };
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
            // VAL tests - include private keys, public keys, Cs, nonce, salt from both parties
            // AFT tests - include, public keys, Cs, nonce, salt from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.IutE),
                nameof(TestCase.IutN),
                nameof(TestCase.ServerE),
                nameof(TestCase.ServerN),
                nameof(TestCase.ServerC),
                nameof(TestCase.IutD),
                nameof(TestCase.IutDmp1),
                nameof(TestCase.IutDmq1),
                nameof(TestCase.IutIqmp),
                nameof(TestCase.IutP),
                nameof(TestCase.IutQ),
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.Z),
                nameof(TestCase.HashZ),
                nameof(TestCase.IutC),
                nameof(TestCase.IutZ),
                nameof(TestCase.IutHashZ),
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

            var includePropertiesValNonBasicKey = new[]
            {
                nameof(TestCase.ServerP),
                nameof(TestCase.ServerQ),
            };
            var keyGenMethodsToIncludePq = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor,
                IfcKeyGenerationMethod.RsaKpg2_crt,
                IfcKeyGenerationMethod.RsaKpg2_primeFactor,
            };

            if (includePropertiesValNonBasicKey.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase) && keyGenMethodsToIncludePq.Contains(testGroup.KeyGenerationMethod))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
            #endregion Conditional Test Case properties
        }
    }
}
