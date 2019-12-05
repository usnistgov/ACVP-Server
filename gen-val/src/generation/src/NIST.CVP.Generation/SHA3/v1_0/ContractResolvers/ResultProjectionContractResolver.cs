using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.SHA3.v1_0.ContractResolvers
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
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Digest),
                nameof(TestCase.ResultsArray)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.DigestLength))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.Function.Equals("shake", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> AlgoArrayResponseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(AlgoArrayResponse.Digest)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            if (jsonProperty.UnderlyingName == nameof(AlgoArrayResponse.DigestLength))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetAlgoArrayResponseFromAlgoArrayResponseObject(instance, out var resp);

                    return resp.ShouldPrintOutputLength;
                };
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

            if (typeof(AlgoArrayResponse).IsAssignableFrom(type))
            {
                return AlgoArrayResponseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }

        private void GetAlgoArrayResponseFromAlgoArrayResponseObject(object instance,
            out AlgoArrayResponse algoArrayResponse)
        {
            algoArrayResponse = instance as AlgoArrayResponse;
        }
    }
}
