using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202.ContractResolvers
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
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength)
            };

            var aftProperties = new[]
            {
                nameof(TestCase.DigestLength)
            };

            if (allProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }
            
            if (aftProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "aft";
                };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
