using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var allProperties = new[]
            {
                nameof(TestCase.TestCaseId)
            };

            var aftMctProperties = new[]
            {
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength)
            };

            var ldtProperties = new[]
            {
                nameof(TestCase.LargeMessage)
            };

            if (allProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Group properties
            if (aftMctProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "mct" || testGroup.TestType.ToLower() == "aft";
                };
            }

            if (ldtProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "ldt";
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
