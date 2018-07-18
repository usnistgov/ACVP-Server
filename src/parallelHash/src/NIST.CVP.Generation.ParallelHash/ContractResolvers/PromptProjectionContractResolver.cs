using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.ParallelHash.ContractResolvers
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

                    if (testGroup.Function.Equals("parallelhash", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldDeserialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message),
                nameof(TestCase.BlockSize),
                nameof(TestCase.Customization),
                nameof(TestCase.MessageLength),
                nameof(TestCase.DigestLength)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
