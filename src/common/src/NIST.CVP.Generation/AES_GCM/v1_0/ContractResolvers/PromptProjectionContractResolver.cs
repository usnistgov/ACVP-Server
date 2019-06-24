using Newtonsoft.Json.Serialization;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;

namespace NIST.CVP.Generation.AES_GCM.v1_0.ContractResolvers
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
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Function),
                nameof(TestGroup.KeyLength),
                nameof(TestGroup.IvLength),
                nameof(TestGroup.IvGeneration),
                nameof(TestGroup.IvGenerationMode),
                nameof(TestGroup.PayloadLength),
                nameof(TestGroup.AadLength),
                nameof(TestGroup.TagLength),
                nameof(TestGroup.Tests)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            if (jsonProperty.UnderlyingName == nameof(TestGroup.PayloadLength))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.IV),
                nameof(TestCase.Key),
                nameof(TestCase.AAD)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.PlainText))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0 && testGroup.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.Tag))
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

            if (jsonProperty.UnderlyingName == nameof(TestCase.CipherText))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.AlgoMode == AlgoMode.AES_GCM_v1_0 && testGroup.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
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