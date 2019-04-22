using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// Include tgId, l, n, hashAlg, test type.  include GGenMode or PrimeGenMode when not "none".
        /// </summary>
        /// <param name="jsonProperty">The property to check</param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.L),
                nameof(TestGroup.N),
                nameof(TestGroup.HashAlgName),
                nameof(TestGroup.TestType)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        
        /// <summary>
        /// Include tcId, message
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message)
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