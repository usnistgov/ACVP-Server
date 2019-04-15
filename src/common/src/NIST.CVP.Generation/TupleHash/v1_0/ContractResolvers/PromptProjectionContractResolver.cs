using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.TupleHash.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Function),
                nameof(TestGroup.HexCustomization),
                nameof(TestGroup.MessageLength),
                nameof(TestGroup.OutputLength),
                nameof(TestGroup.XOF),
                nameof(TestGroup.Tests),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Tuple),
                nameof(TestCase.MessageLength)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.Customization))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        if (testGroup.HexCustomization)
                        {
                            return false;
                        }

                        return true;
                    }
                    return false;
                };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.CustomizationHex))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        if (testGroup.HexCustomization)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.DigestLength))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    return false;
                };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
