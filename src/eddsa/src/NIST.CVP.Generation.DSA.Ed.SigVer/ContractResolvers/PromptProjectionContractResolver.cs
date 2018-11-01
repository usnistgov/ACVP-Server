using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// Include tgId, l, n, hashAlg, test type.
        /// </summary>
        /// <param name="jsonProperty">The property to check</param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.TestType),
                nameof(TestGroup.PreHash),
                nameof(TestGroup.Curve)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }
            return jsonProperty.ShouldSerialize = instance => false;
        }
        
        /// <summary>
        /// Include tcId, message, y, r, s
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.Message),
                nameof(TestCase.Sig),
                nameof(TestCase.Q)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.Context))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if ((!testGroup.PreHash) && testGroup.Curve == Crypto.Common.Asymmetric.DSA.Ed.Enums.Curve.Ed25519)
                    {
                        return false;
                    }

                    return true;
                };
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}