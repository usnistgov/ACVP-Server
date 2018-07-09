using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;

namespace NIST.CVP.Generation.RSA_DPComponent.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // Exclude cipherText
            var excludeProperties = new[]
            {
                nameof(TestCase.Deferred)
            };

            if (excludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => false;
            }

            return jsonProperty.ShouldSerialize = instance => true;
        }

        private Predicate<object> AlgoArrayResponseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(AlgoArrayResponseSignature.PlainText),
                nameof(AlgoArrayResponseSignature.E),
                nameof(AlgoArrayResponseSignature.N),
                nameof(AlgoArrayResponseSignature.FailureTest)
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
