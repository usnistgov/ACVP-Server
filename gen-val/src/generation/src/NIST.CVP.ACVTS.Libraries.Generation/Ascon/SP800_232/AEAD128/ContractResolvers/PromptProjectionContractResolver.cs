using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Direction),
                nameof(TestGroup.Tests),
                nameof(TestGroup.NonceMasking),
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
                nameof(TestCase.Key),
                nameof(TestCase.Nonce),
                nameof(TestCase.AD),
                nameof(TestCase.PayloadBitLength),
                nameof(TestCase.ADBitLength),
                nameof(TestCase.TagLength),
                nameof(TestCase.SecondKey),
            };

            var includeEncryptProperties = new[]
            {
                nameof(TestCase.Plaintext)
            };

            var includeDecryptProperties = new[]
            {
                nameof(TestCase.Tag),
                nameof(TestCase.Ciphertext),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            if (includeDecryptProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                    return group.Direction == BlockCipherDirections.Decrypt;
                };
            }

            if (includeEncryptProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                    return group.Direction == BlockCipherDirections.Encrypt;
                };
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }
    }
}
