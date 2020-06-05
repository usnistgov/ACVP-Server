using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc.ContractResolvers
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
                nameof(TestGroup.HashFunctionZ),
                nameof(TestGroup.Tests),
            };
            
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
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
                nameof(TestCase.Z),
                nameof(TestCase.HashZ)
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
            
            var valIncludeZ = new[]
            {
                nameof(TestCase.Z),
            };
            if (valIncludeZ.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
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
            
            var valIncludeHashZ = new[]
            {
                nameof(TestCase.HashZ),
            };
            if (valIncludeHashZ.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
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