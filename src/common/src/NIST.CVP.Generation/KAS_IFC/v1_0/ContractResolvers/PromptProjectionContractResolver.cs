using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;

namespace NIST.CVP.Generation.KAS_IFC.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, Cs, nonce, salt from both parties
            // AFT tests - include, public keys, Cs, nonce, salt from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.IutE),
                nameof(TestCase.IutN),
                nameof(TestCase.ServerE),
                nameof(TestCase.ServerN),
                nameof(TestCase.ServerC),
                nameof(TestCase.ServerNonce),
                nameof(TestCase.Salt),
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.ServerD),
                nameof(TestCase.ServerP),
                nameof(TestCase.ServerQ),
                nameof(TestCase.ServerDmp1),
                nameof(TestCase.ServerDmq1),
                nameof(TestCase.ServerIqmp),
                nameof(TestCase.IutD),
                nameof(TestCase.IutP),
                nameof(TestCase.IutQ),
                nameof(TestCase.IutDmp1),
                nameof(TestCase.IutDmq1),
                nameof(TestCase.IutIqmp),
                nameof(TestCase.Tag)
            };

            if (includePropertiesValScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
            #endregion Conditional Test Case properties
        }
    }
}
