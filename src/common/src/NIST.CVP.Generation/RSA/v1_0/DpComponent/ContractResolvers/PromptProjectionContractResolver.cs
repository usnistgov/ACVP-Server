using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent.ContractResolvers
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
            return jsonProperty.ShouldSerialize = instance => true;
        }

        private Predicate<object> AlgoArrayResponseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(AlgoArrayResponseSignature.CipherText)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            if (typeof(AlgoArrayResponseSignature).IsAssignableFrom(type))
            {
                return AlgoArrayResponseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
    }
}
