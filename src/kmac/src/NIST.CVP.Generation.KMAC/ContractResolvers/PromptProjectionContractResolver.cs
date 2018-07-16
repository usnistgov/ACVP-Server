using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.KMAC.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.TestType),
                nameof(TestGroup.KeyLength),
                nameof(TestGroup.MessageLength)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Key),
                nameof(TestCase.Message),
                nameof(TestCase.Customization)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
