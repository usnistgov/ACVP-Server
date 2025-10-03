using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
            };

            var includeEncryptProperties = new[]
            {
                nameof(TestCase.Ciphertext),
                nameof(TestCase.Tag)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            if (includeEncryptProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                    return group.Direction == Crypto.Common.Symmetric.Enums.BlockCipherDirections.Encrypt;
                };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.TestPassed))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        // only write the "testPassed: false" when the test case is a failure test.
                        if (EnumHelpers.GetEnumDescriptionFromEnum(testGroup.Direction) == "decrypt")
                            //&& (testCase.TestPassed != null && !testCase.TestPassed.Value))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.Plaintext))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        // only write the "testPassed: false" when the test case is a failure test.
                        if (EnumHelpers.GetEnumDescriptionFromEnum(testGroup.Direction) == "decrypt"
                            && (testCase.TestPassed != null && testCase.TestPassed.Value && testCase.Plaintext != null))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }
    }
}
