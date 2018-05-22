using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.DRBG.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// Every group property included
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize = instance => true;
        }

        
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var excludeProperties = new[]
            {
                nameof(TestCase.ReturnedBits),
                nameof(TestCase.Deferred),
                nameof(TestCase.TestPassed),
            };

            if (excludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => false;
            }
            
            return jsonProperty.ShouldSerialize = instance => true;
        }
    }
}