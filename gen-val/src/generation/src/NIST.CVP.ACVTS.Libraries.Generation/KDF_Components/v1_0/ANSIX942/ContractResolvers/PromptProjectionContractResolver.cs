using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942.ContractResolvers
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
            var derProperties = new[]
            {
                nameof(TestGroup.Oid)
            };

            if (derProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);
                    return testGroup.KdfType == AnsiX942Types.Der;                // Only include OID if KDF is DER
                };
            }

            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Zz),
                nameof(TestCase.KeyLen)
            };

            var concatProperties = new[]
            {
                nameof(TestCase.OtherInfo)
            };

            var derProperties = new[]
            {
                nameof(TestCase.PartyUInfo),
                nameof(TestCase.PartyVInfo),
                nameof(TestCase.SuppPubInfo),
                nameof(TestCase.SuppPrivInfo)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            if (derProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);
                    return testGroup.KdfType == AnsiX942Types.Der;
                };
            }

            if (concatProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);
                    return testGroup.KdfType == AnsiX942Types.Concat;
                };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
