using System;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using System.Linq;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId), 
                nameof(TestGroup.TestType), 
                nameof(TestGroup.LmsMode),
                nameof(TestGroup.LmOtsMode), 
                nameof(TestGroup.Tests),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId), 
                nameof(TestCase.Seed), 
                nameof(TestCase.I)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }
    }
}
