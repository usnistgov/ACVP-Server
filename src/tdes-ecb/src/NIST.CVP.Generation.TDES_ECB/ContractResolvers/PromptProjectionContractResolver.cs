using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.TDES_ECB.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldDeserialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Key1),
                nameof(TestCase.Key2),
                nameof(TestCase.Key3)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.PlainText))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.CipherText))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
