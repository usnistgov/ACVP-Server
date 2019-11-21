using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.SHA3.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            #region Conditional Test Group properties
            if (jsonProperty.UnderlyingName == nameof(TestGroup.BitOrientedOutput))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);

                    if (testGroup.Function.Equals("shake", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }

            var outputLengths = new[] {nameof(TestGroup.MinOutputLength), nameof(TestGroup.MaxOutputLength)};
            if (outputLengths.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);

                    if (testGroup.Function.Equals("shake", StringComparison.OrdinalIgnoreCase) &&
                        testGroup.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Group properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.DigestLength))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.Function.Equals("shake", StringComparison.OrdinalIgnoreCase) &&
                        (testGroup.TestType.Equals("vot", StringComparison.OrdinalIgnoreCase) || testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }

                    return false;
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
