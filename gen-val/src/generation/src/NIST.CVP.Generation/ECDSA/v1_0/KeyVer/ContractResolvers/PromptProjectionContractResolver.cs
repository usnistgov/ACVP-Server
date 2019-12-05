using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.ECDSA.v1_0.KeyVer.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// include id, type, tests, l, and n
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Tests),
                nameof(TestGroup.Curve),
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
                nameof(TestCase.Qx),
                nameof(TestCase.Qy),
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
