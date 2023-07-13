using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Tests)
            };

            var mctProperties = new[]
            {
                nameof(TestGroup.MctVersion)
            };
            
            #region Conditional Test Group properties
            if (mctProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);

                    if (testGroup.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            
            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }
            #endregion Conditional Test Group properties
            
            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var allProperties = new[]
            {
                nameof(TestCase.TestCaseId)
            };

            var includePropertiesAftMct = new[]
            {
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength)
            };

            var includePropertiesLdt = new[]
            {
                nameof(TestCase.LargeMessage)
            };

            if (allProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            if (includePropertiesAftMct.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "mct" || testGroup.TestType.ToLower() == "aft";
                };
            }

            if (includePropertiesLdt.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "ldt";
                };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
