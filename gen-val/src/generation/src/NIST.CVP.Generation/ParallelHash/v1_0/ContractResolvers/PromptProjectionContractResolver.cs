﻿using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.ParallelHash.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Function),
                nameof(TestGroup.HexCustomization),
                nameof(TestGroup.XOF),
                nameof(TestGroup.Tests),
            };
            
            var mctProperties = new[]
            {
                nameof(TestGroup.MinOutputLength),
                nameof(TestGroup.MaxOutputLength)
            };

            if (mctProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);
                    return testGroup.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase);
                };
            }

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
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength),
                nameof(TestCase.BlockSize)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.Customization))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);
                    return !testGroup.HexCustomization;
                };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.CustomizationHex))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);
                    return testGroup.HexCustomization;
                };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.DigestLength))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
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
