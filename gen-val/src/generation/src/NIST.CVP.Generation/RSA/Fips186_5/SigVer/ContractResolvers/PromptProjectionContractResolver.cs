using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigVer.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.Mode),
                nameof(TestGroup.TestType),
                nameof(TestGroup.HashAlgName),
                nameof(TestGroup.N),
                nameof(TestGroup.E),
                nameof(TestGroup.Modulo)
            };

            var pssIncludeProperties = new[]
            {
                nameof(TestGroup.MaskFunction),
                nameof(TestGroup.SaltLen)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional group properties
            if (jsonProperty.UnderlyingName == nameof(TestGroup.Conformance))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);
                        return testGroup.IsMessageRandomized;
                    };
            }
            
            if (pssIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);
                        return testGroup.Mode == SignatureSchemes.Pss;
                    };
            }
            #endregion Conditional group properties

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message),
                nameof(TestCase.Signature)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.RandomValue))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        return testGroup.IsMessageRandomized;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.RandomValueLen))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        return testGroup.IsMessageRandomized;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}